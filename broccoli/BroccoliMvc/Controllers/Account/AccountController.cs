using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using BroccoliTrade.Domain;
using BroccoliTrade.Domain.Models;
using BroccoliTrade.Logics.Infrastructure.Extensions;
using BroccoliTrade.Logics.Interfaces.Membership;
using BroccoliTrade.Logics.Interfaces.Statistics;
using BroccoliTrade.Logics.MSMQ;
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

                    return RedirectToAction("Index", "PersonalCabinet");
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
                        Password = model.Password,
                        RoleId = 2,
                        GroupId = 1,
                        MailNumber = 1
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

                if (Request.Cookies["banner"] != null)
                {
                    user.FromBanner = Convert.ToInt32(Request.Cookies["banner"].Value);
                }

                _usersService.Insert(user);
                _membershipService.LoginUser(null, user.Email, model.Password, false);

                var em = new EmailMessage
                {
                    Subject = "Новый пользователь Broccoli-trade [f000]",
                    Message = string.Format("ФИО: {0}<br/>" +
                                            "Ник: {1}<br/>" +
                                            "Email: {2}<br/>" +
                                            "<b>Тел.:</b> {3}<br/>" +
                                            "Страна: {4}<br/>" +
                                            "Город: {5}<br/>" +
                                            "Дата рождения: {6}<br/><br/>" +
                                            "Посмотреть других новых пользователей <a href=\"http://broccoli-trade.ru/Admin/Users\">www.broccoli-trade.ru</a>.<br/>" +
                                            "Посмотреть статистику партнерских счетов у <a href=\"https://my.forexinn.ru/accounts\">Forexinn</a>", 
                                            user.Name,
                                            user.Nickname,
                                            user.Email,
                                            user.Phone,
                                            user.Country,
                                            user.City,
                                            user.BirthDay.ToShortDateString()
                                            ),
                    From = "support@broccoli-trade.ru",
                    DisplayNameFrom = "Broccoli Trade",
                    To = "broccoli@molchunov.com"
                };

                var emToUser = new EmailMessage
                    {
                        Subject = string.Format("Детали учетной записи для {0}", user.Name),
                        Message = string.Format("Здравствуйте, {0},<br/>" +
                                                "Спасибо за вашу регистрацию на Broccoli Trade<br/>" +
                                                "Вы можете войти на <a href=\"www.broccoli-trade.ru\">www.broccoli-trade.ru</a> " +
                                                "используя<br/><br/>" +
                                                "<b>Email: </b>{1}<br/>" +
                                                "<b>Пароль: </b>{2}",
                                                user.Name,
                                                user.Email,
                                                model.Password),
                        From = "support@broccoli-trade.ru",
                        DisplayNameFrom = "Broccoli Trade",
                        To = user.Email
                    };

                new QueueService().QueueMessage(em);
                new QueueService().QueueMessage(emToUser);

                return RedirectToAction("Index", "PersonalCabinet");
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

        public ActionResult ForgotPassword(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
            }

            return View();
        }

        public ActionResult PasswordRecovery(string email)
        {
            var user = _usersService.GetUserByLogin(email);

            if (user != null)
            {
                var pass = GeneratePassword(6);

                user.Password = pass.Md5();
                _usersService.Update(user);
                _usersService.SaveChanges();

                var em = new EmailMessage
                    {
                        Subject = "Восстановление пароля",
                        Message = string.Format("Ваш новый пароль: {0}", pass),
                        From = "support@broccoli-trade.ru",
                        DisplayNameFrom = "Broccoli Trade",
                        To = email
                    };

                new QueueService().QueueMessage(em);

                return View("PasswordSent");
            }

            return RedirectToAction("ForgotPassword", "Account", new { @error = "Не найден указанный email" });
        }

        public ActionResult PasswordSent()
        {
            return View();
        }

        private string GeneratePassword(int count)
        {
            var random = new Random(); //Random для генерирования пароля
            var password = string.Empty; //Строка для пароля

            //Генерируем пароль
            for (var i = 0; i < count; i++)
            {
                var ch = Convert.ToChar(random.Next(97, 122));
                password += ch;
            }

            return password;
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
