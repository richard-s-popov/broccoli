using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BroccoliTrade.Domain;
using BroccoliTrade.Domain.Models;
using BroccoliTrade.Logics.Interfaces.Communications;
using BroccoliTrade.Logics.Interfaces.Membership;
using BroccoliTrade.Logics.MSMQ;
using BroccoliTrade.Web.BroccoliMvc.Infrastructure.Attributes;
using BroccoliTrade.Web.BroccoliMvc.Models.Account;
using BroccoliTrade.Web.BroccoliMvc.Models.Admin;
using BroccoliTrade.Web.BroccoliMvc.Models.Communications;

namespace BroccoliTrade.Web.BroccoliMvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly ICommunicationService _communicationService;

        public AdminController(
            IUsersService usersService,
            ICommunicationService communicationService)
        {
            _usersService = usersService;
            _communicationService = communicationService;
        }

        [Secure]
        [Role(Role = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Secure]
        [Role(Role = "Admin")]
        public ActionResult CommentsPanel()
        {
            var comments = _communicationService.GetAllNewComments();
            var model = new CommentsListModel
            {
                CommentList = comments.Select(x => new CommentModel
                {
                    Id = x.Id,
                    UserName = x.Users.Name,
                    Body = x.Body
                })
            };

            return View(model);
        }

        [Secure]
        [Role(Role = "Admin")]
        public JsonResult ApproveComment(int id)
        {
            var comment = _communicationService.GetById(id);

            if (comment == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

            comment.IsConfirmed = true;
            _communicationService.SaveChanges(); ;

            return Json(new {result = true}, JsonRequestBehavior.AllowGet);
        }

        [Secure]
        [Role(Role = "Admin")]
        public JsonResult DeclineComment(int id)
        {
            var comment = _communicationService.GetById(id);

            if (comment == null)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

            comment.IsDeleted = true;
            _communicationService.SaveChanges(); ;

            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        [Secure]
        [Role(Role = "Admin")]
        public ActionResult Users()
        {
            var users = _usersService.GetAllUsers().OrderByDescending(x => x.RegisterDate);
            var model = new UsersListModel
                {
                    users = users.Select(entity => new UserProfilePoco
                        {
                            Id = entity.Id,
                            Email = entity.Email,
                            RegisterDate = entity.RegisterDate,
                            Name = entity.Name,
                            City = entity.City,
                            Phone = entity.Phone,
                            From = entity.FromBanner != null ? entity.FromBanner.Value : 0
                        })
                };

            return View(model);
        }

        [Secure]
        [Role(Role = "Admin")]
        public ActionResult SendEmails()
        {
            var mailGroups = _communicationService.GetAllGroups().ToList();

            var model = new MailGroupsListModel
                {
                    GroupList = mailGroups.Select(g => new MailGroup
                        {
                            GroupName = g.Name,
                            Id = g.Id,
                            MailList = g.Mails.Where(x => !x.IsDeleted).Select(m => new Mail
                                {
                                    Id = m.Id,
                                    MailBody = m.MailBody.Substring(0, m.MailBody.Length > 100 ? 100 : m.MailBody.Length),
                                    MailNumber = m.MailNumber,
                                    MailName = m.MailName
                                })
                        })
                };

            return View("SendEmailsPanel", model);
        }

        [Secure]
        [Role(Role = "Admin")]
        public ActionResult NewMail(int groupId)
        {
            var group = _communicationService.GetGroupById(groupId);
            
            ViewBag.GroupName = group.Name;
            ViewBag.GroupId = group.Id;

            return View();
        }

        [Secure]
        [Role(Role = "Admin")]
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveMail(int? id, string subject, string body, int groupId)
        {
            var entity = new Mails();

            if (id.HasValue)
            {
                entity = _communicationService.GetMailById(id.Value);

                entity.MailName = subject;
                entity.MailBody = body;
                entity.IsDeleted = false;
            }
            else
            {
                var maxNumber = _communicationService.GetNextNumberInGroup(groupId);

                entity = new Mails
                {
                    GroupId = groupId,
                    MailBody = body,
                    MailName = subject,
                    MailNumber = maxNumber
                };

                _communicationService.SaveMail(entity);
            }

            _communicationService.SaveChanges();

            return Json(new {result = true, mailId = entity.Id});
        }

        [Secure]
        [Role(Role = "Admin")]
        public ActionResult EditMail(int mailId)
        {
            var mail = _communicationService.GetMailById(mailId);
            var group = _communicationService.GetGroupById(mail.GroupId);

            ViewBag.GroupName = group.Name;
            ViewBag.GroupId = group.Id;

            return View(new Mail
                {
                    Id = mail.Id,
                    MailName = mail.MailName,
                    MailBody = mail.MailBody,
                    MailNumber = mail.MailNumber
                });
        }

        [Secure]
        [Role(Role = "Admin")]
        public ActionResult DeleteMail(int mailId)
        {
            var entity = _communicationService.GetMailById(mailId);

            try
            {
                entity.IsDeleted = true;

                _communicationService.SaveChanges();

                return Json(new {result = true}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [Secure]
        [Role(Role = "Admin")]
        public ActionResult WriteMailForm()
        {
            var groups = this._communicationService.GetAllGroups();
            var model = new MailGroupsListModel
                {
                    GroupList = groups.Select(x => new MailGroup
                        {
                            GroupName = x.Name,
                            Id = x.Id
                        })
                };

            return View("WriteMail", model);
        }

        [Secure]
        [Role(Role = "Admin")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult WriteMailFinish(string subject, string body, string groups)
        {
            var groupIds = groups.Split(';');

            foreach (var groupId in groupIds)
            {
                var group = this._communicationService.GetGroupById(Convert.ToInt32(groupId));
                var users = group.Users.ToList();
                
                foreach (var user in users)
                {
                    var em = new EmailMessage
                    {
                        Subject = subject.Replace("%ИМЯ%", user.Name),
                        Message = body.Replace("%ИМЯ%", user.Name),
                        From = "support@broccoli-trade.ru",
                        DisplayNameFrom = "Broccoli Trade",
                        To = user.Email
                    };

                    new QueueService().QueueMessage(em);
                }
            }

            return RedirectToAction("SendEmails");
        }

        protected override void Dispose(bool disposing)
        {
            _usersService.Dispose();
            _communicationService.Dispose();
            base.Dispose(disposing);
        }
    }
}
