﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BroccoliTrade.Domain;
using BroccoliTrade.Logics.Interfaces.Membership;
using BroccoliTrade.Web.BroccoliMvc.Infrastructure.Attributes;
using BroccoliTrade.Web.BroccoliMvc.Models.Account;

namespace BroccoliTrade.Web.BroccoliMvc.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;

        private readonly IMembershipService _membershipService;

        public AccountController(
            IUsersService usersService,
            IMembershipService membershipService)
        {
            _usersService = usersService;
            _membershipService = membershipService;
        }

        public ActionResult RegisterForm()
        {
            return this.View();
        }

        public ActionResult LogOn()
        {
            return this.View();
        }

        public ActionResult Login(string login, string pass, bool rememberMe, string returnUrl)
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(pass))
            {
                if (_membershipService.AuthorizeUser(login, pass))
                {
                    _membershipService.LoginUser(null, login, pass, rememberMe);

                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return Redirect(HttpContext.Request.UrlReferrer.AbsolutePath);
                }
            }

            return Json(new { authorize = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            _membershipService.LogoutCurrentUser(null);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult FinishRegister(UserProfilePoco model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users
                    {
                        Name = model.Name,
                        Nickname = model.Nickname,
                        Email = model.Email,
                        Phone = model.Phone,
                        Country = model.Country,
                        City = model.City,
                        BirthDay = model.BirthDay,
                        Password = model.Password
                    };

                _usersService.Insert(user);

                return RedirectToAction("Index", "Home");
            }

            return this.View("RegisterForm");
        }

        public JsonResult NicknameIsExist(string nickname)
        {
            var isExist = _usersService.NicknameIsExist(nickname);

            return Json(new { result = isExist }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmailIsExist(string email)
        {
            var isExist = _usersService.EmailIsExist(email);

            return Json(new { result = isExist }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _usersService.Dispose();
            _membershipService.Dispose();
            base.Dispose(disposing);
        }
    }
}