﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BroccoliTrade.Domain;
using BroccoliTrade.Logics.Interfaces.Membership;
using BroccoliTrade.Logics.Interfaces.Statistics;
using BroccoliTrade.Web.BroccoliMvc.Infrastructure.Attributes;
using BroccoliTrade.Web.BroccoliMvc.Models.Account;

namespace BroccoliTrade.Web.BroccoliMvc.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IStatService _statService;
        private readonly IMembershipService _membershipService;

        public AccountController(
            IUsersService usersService,
            IMembershipService membershipService,
            IStatService statService)
        {
            _usersService = usersService;
            _membershipService = membershipService;
            _statService = statService;
        }

        public ActionResult RegisterForm()
        {
            var days = new List<SelectListItem>
                {
                    new SelectListItem {Text = "-", Value = "0"}
                };
            days.AddRange(Enumerable.Range(1, 31).Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.ToString()
                }));

            var months = new List<SelectListItem>
                {
                    new SelectListItem {Text = "-", Value = "0"}
                };
            if (DateTimeFormatInfo.CurrentInfo != null)
            {
                months.AddRange(DateTimeFormatInfo
                                    .CurrentInfo
                                    .MonthNames
                                    .Select((monthName, index) => new SelectListItem
                                        {
                                            Value = (index + 1).ToString(),
                                            Text = monthName
                                        }));
            }
            else
            {
                months.AddRange(DateTimeFormatInfo
                                    .InvariantInfo
                                    .MonthNames
                                    .Select((monthName, index) => new SelectListItem
                                    {
                                        Value = (index + 1).ToString(),
                                        Text = monthName
                                    }));
            }
            months.Remove(months.Last());

            var years = new List<SelectListItem>
                {
                    new SelectListItem {Text = "-", Value = "0"}
                };
            years.AddRange(Enumerable.Range(DateTime.Now.Year - 60, 42).OrderByDescending(x => x)
                .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString()
                    }));

            ViewBag.Days = new SelectList(days, "Value", "Text");
            ViewBag.Month = new SelectList(months, "Value", "Text");
            ViewBag.Years = new SelectList(years, "Value", "Text");

            return this.View();
        }

        public ActionResult LogOn()
        {
            if (HttpContext.Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "PersonalCabinet");
            }

            return this.View();
        }

        public ActionResult Login(string login, string pass, bool rememberMe, bool? isLoginPage, string returnUrl)
        {
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(pass))
            {
                if (_membershipService.AuthorizeUser(login, pass))
                {
                    _membershipService.LoginUser(null, login, pass, rememberMe);

                    if (isLoginPage != null && isLoginPage.Value)
                    {
                        return RedirectToAction("Index", "PersonalCabinet");
                    }

                    return Redirect(!String.IsNullOrEmpty(returnUrl) ? returnUrl : HttpContext.Request.UrlReferrer.AbsolutePath);
                }
            }

            return RedirectToAction("LogOn");
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

                if (Request.Cookies["owner"] != null)
                {
                    user.OwnerId = Convert.ToInt64(Request.Cookies["owner"].Value);

                    var owner = _usersService.GetById((long)user.OwnerId);
                    if (owner != null)
                    {
                        // Если в куках есть информация о реферальном url, то добавляем эту информацию в статистику
                        var hostCookie = Request.Cookies["Host"];
                        if (hostCookie != null)
                        {
                            var host = hostCookie["shortHost"];
                            var fullHostUrl = hostCookie["fullHostUrl"];

                            var entity = new Referrer
                                {
                                    Host = host,
                                    FullReferrerUrl = fullHostUrl,
                                    Date = DateTime.Now,
                                    IsDeleted = false,
                                    OwnerId = owner.Id,
                                    Registered = true
                                };

                            _statService.AddReferrer(entity);
                        }
                    }
                }

                _usersService.Insert(user);

                return RedirectToAction("LogOn", "Account");
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
            _statService.Dispose();
            base.Dispose(disposing);
        }
    }
}
