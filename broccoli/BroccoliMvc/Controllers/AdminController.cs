using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BroccoliTrade.Logics.Interfaces.Communications;
using BroccoliTrade.Logics.Interfaces.Membership;
using BroccoliTrade.Web.BroccoliMvc.Infrastructure.Attributes;
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

        protected override void Dispose(bool disposing)
        {
            _usersService.Dispose();
            _communicationService.Dispose();
            base.Dispose(disposing);
        }
    }
}
