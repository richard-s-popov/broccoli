using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BroccoliTrade.Domain;
using BroccoliTrade.Domain.Models;
using BroccoliTrade.Logics.Interfaces.Communications;
using BroccoliTrade.Logics.Interfaces.Membership;
using BroccoliTrade.Logics.Interfaces.Statistics;
using BroccoliTrade.Logics.MSMQ;
using BroccoliTrade.Web.BroccoliMvc.Models.Communications;

namespace BroccoliTrade.Web.BroccoliMvc.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IStatService _statService;
        private readonly ICommunicationService _communicationService;

        public HomeController(
            IUsersService usersService,
            IStatService statService,
            ICommunicationService communicationService)
        {
            _usersService = usersService;
            _statService = statService;
            _communicationService = communicationService;
        }

        public ActionResult FromBanner(string id)
        {
            var cookie = new HttpCookie("banner");
            cookie.Value = id;
            cookie.Expires = DateTime.Now.AddHours(24);
            ControllerContext.HttpContext.Response.Cookies.Add(cookie);

            return RedirectToAction("Index");
        }

        public ActionResult Index(string token)
        {
            var ownerUser = _usersService.GetUserByEmailHash(token);
            
            if (ownerUser != null)
            {
                if (Request.Cookies["owner"] == null)
                {
                    var cookie = new HttpCookie("owner");
                    cookie.Value = ownerUser.Id.ToString(CultureInfo.InvariantCulture);
                    cookie.Expires = DateTime.Now.AddHours(1);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                    // Если есть реферальный url, то сохраняем в статистику
                    if (HttpContext.Request.UrlReferrer != null)
                    {
                        var urlReferrer = HttpContext.Request.UrlReferrer;

                        // Создаем новые cookie
                        if (Request.Cookies["Host"] == null)
                        {
                            var hostCookie = new HttpCookie("Host");
                            hostCookie.Values["shortHost"] = urlReferrer.Host;
                            hostCookie.Values["fullHostUrl"] = urlReferrer.OriginalString;
                            hostCookie.Expires = DateTime.Now.AddHours(1);
                            ControllerContext.HttpContext.Response.Cookies.Add(hostCookie);
                        }
                        else // или обновляем имеющиеся
                        {
                            Response.Cookies["Host"]["shortHost"] = urlReferrer.Host;
                            Response.Cookies["Host"]["fullHostUrl"] = urlReferrer.OriginalString;
                            Response.Cookies["Host"].Expires = DateTime.Now.AddHours(1);
                        }

                        var entity = new Referrer
                        {
                            Host = urlReferrer.Host,
                            FullReferrerUrl = urlReferrer.OriginalString,
                            Date = DateTime.Now,
                            IsDeleted = false,
                            OwnerId = ownerUser.Id,
                            Registered = false
                        };

                        _statService.AddReferrer(entity);
                    }
                    else
                    {
                        Response.Cookies["Host"]["shortHost"] = "undefined";
                        Response.Cookies["Host"]["fullHostUrl"] = "undefined";
                        Response.Cookies["Host"].Expires = DateTime.Now.AddHours(1);

                        var entity = new Referrer
                        {
                            Host = "undefined",
                            FullReferrerUrl = "",
                            Date = DateTime.Now,
                            IsDeleted = false,
                            OwnerId = ownerUser.Id,
                            Registered = false
                        };

                        _statService.AddReferrer(entity);
                    }
                }
                else
                {
                    Response.Cookies["owner"].Value = ownerUser.Id.ToString(CultureInfo.InvariantCulture);
                    Response.Cookies["owner"].Expires = DateTime.Now.AddHours(1);
                }
            }

            return View();
        }

        public ActionResult Comments()
        {
            var comments = _communicationService.GetAllConfirmedComments().OrderByDescending(x => x.Date);
            var model = new CommentsListModel
                {
                    CommentList = comments.Select(x => new CommentModel
                        {
                            UserName = x.Users.Name.Trim(),
                            Body = x.Body,
                            Date = x.Date
                        })
                };

            return View(model);
        }

        [ValidateInput(false)]
        public JsonResult AddComment(string comment)
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);

            if (currentUser == null)
            {
                return Json(new {result = false}, JsonRequestBehavior.AllowGet);
            }

            _communicationService.AddComment(new Comment
                {
                    UserId = currentUser.Id,
                    Body = comment,
                    Date = DateTime.Now,
                    IsConfirmed = currentUser.RoleId == 1,
                    IsDeleted = false
                });

            return Json(new {result = true}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VPS()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult SendContact(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                var em = new EmailMessage
                {
                    Subject = model.Subject,
                    Message = string.Format("{0}\n{1}\n{2}", model.Message, model.Name, model.Email),
                    From = "broccoli2@molchunov.com",
                    DisplayNameFrom = "Обратная связь",
                    To = "broccoli@molchunov.com"
                };

                new EmailService().SendMessage(em,
                    "broccoli2@molchunov.com",
                    "123456aaa111",
                    "smtp.yandex.ru",
                    587,
                    true);  
            }

            return View("ContactUs");
        }

        public ActionResult PAMM()
        {
            return View();
        }

        public ActionResult BrokerInfo()
        {
            return View();
        }

        public ActionResult BroccoliOffer()
        {
            return View();
        }

        public ActionResult Partners()
        {
            return View();
        }

        public ActionResult Partner(int id)
        {
            return RedirectToAction("FromBanner", new { @id = 1024 });
        }

        public ActionResult MaxTrade()
        {
            return View("Advisors/MaxTrade");
        }

        public ActionResult GarantedProfit()
        {
            return View("Advisors/GarantedProfit");
        }

        protected override void Dispose(bool disposing)
        {
            _usersService.Dispose();
            _statService.Dispose();
            _communicationService.Dispose();
            base.Dispose(disposing);
        }
    }
}
