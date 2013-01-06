﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BroccoliTrade.Domain;
using BroccoliTrade.Domain.Models;
using BroccoliTrade.Logics.Infrastructure.Extensions;
using BroccoliTrade.Logics.Interfaces.Account;
using BroccoliTrade.Logics.Interfaces.Membership;
using BroccoliTrade.Logics.Interfaces.Statistics;
using BroccoliTrade.Logics.Interfaces.TradingSystem;
using BroccoliTrade.Logics.MSMQ;
using BroccoliTrade.Web.BroccoliMvc.Infrastructure.Attributes;
using BroccoliTrade.Web.BroccoliMvc.Models.PersonalCabinet;

namespace BroccoliTrade.Web.BroccoliMvc.Controllers.PersonalCabinet
{
    public class PersonalCabinetController : Controller
    {
        private readonly IUsersService _usersService;

        private readonly IAccountsService _accountsService;

        private readonly ITradingSystemService _tradingSystemService;

        private readonly IStatService _statService;

        public PersonalCabinetController(
            IUsersService usersService,
            IAccountsService accountsService,
            ITradingSystemService tradingSystemService,
            IStatService statService)
        {
            _usersService = usersService;
            _accountsService = accountsService;
            _tradingSystemService = tradingSystemService;
            _statService = statService;
        }

        [Secure]
        public ActionResult Index()
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            ViewBag.User = currentUser;

            this.PrepareNotificationsForView();

            return View("PersonalCabinet");
        }

        [Secure]
        public ActionResult ActivateAccount()
        {
            this.PrepareNotificationsForView();

            return this.View();
        }

        [Secure]
        public ActionResult ActivateAccountFinish(string account)
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);

            var accountEntity = new Accounts
                {
                    AccountNumber = account,
                    CreateDate = DateTime.Now,
                    StatusId = 1,
                    UserId = currentUser.Id,
                    IsDeleted = false,
                    IsNew = false
                };

            _accountsService.CreateAccount(accountEntity);

            var applicationEntity = new AccountPool
            {
                AccountNumber = account,
                UserId = currentUser.Id,
                ApplicationDate = DateTime.Now,
                AccountId = accountEntity.Id
            };

            _accountsService.AddAccountToApplicationsPool(applicationEntity);

            return RedirectToAction("AccountStatus");
        }

        [Secure]
        public ActionResult AccountStatus()
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            var accounts = _accountsService.GetAccountsByUserId(currentUser.Id).ToList();

            foreach (var account in accounts)
            {
                account.IsNew = false;
            }
            _accountsService.Save();

            var model = new AccountListPoco
                {
                    accounts = accounts.Select(entity => new AccountRowModel
                        {
                            Number = entity.AccountNumber,
                            Status = entity.Status.Name,
                            StatusId = entity.StatusId
                        })
                };

            ViewBag.NoData = !accounts.Any();
            this.PrepareNotificationsForView();

            return this.View(model);
        }

        [Secure]
        public JsonResult DeleteAccount(string account)
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            var currentAccount = _accountsService.GetByAccountNumber(account);
            
            if (currentAccount.UserId == currentUser.Id)
            {
                _accountsService.DeleteAccount(currentAccount);

                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new {result = false, reason = "Нет прав доступа для удаления данного аккаунта"},
                        JsonRequestBehavior.AllowGet);
        }

        [Secure]
        public ActionResult OrderStatus()
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            var tSystemsEntities = _tradingSystemService.GetTradingSystemsByUserId(currentUser.Id).ToList();

            // Обнуляем сущности как просмотренные
            foreach (var tSystemsEntity in tSystemsEntities)
            {
                tSystemsEntity.IsNew = false;
            }
            _tradingSystemService.Save();

            var model = new TradingSystemListPoco
                {
                    TradingSystems = tSystemsEntities.Select(entity => new TradingSystemRowModel
                        {
                            System = entity.Systems.Name,
                            AccountNumber = entity.Accounts.AccountNumber,
                            Status = entity.Status.Name,
                            Link = entity.Status.Id == 2 ? this.GetLinkForTradingSystem(currentUser.Id, entity.Systems.Id) : "#",
                            LinkText = entity.Status.Id == 2 ? "Ссылка" : string.Empty
                        })
                };

            ViewBag.NoData = !tSystemsEntities.Any();
            this.PrepareNotificationsForView();

            return this.View(model);
        }

        [Secure]
        public ActionResult TradingSystemOrder(int id)
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            var tSystem = _tradingSystemService.GetSystemById(id);
            var activatedAccounts = _accountsService.GetAccountsByUserId(currentUser.Id).Where(x => x.Status.Id == 2).ToList();

            ViewBag.Error = false;

            if (tSystem == null)
            {
                ViewBag.Error = true;
                return this.View();
            }

            if (!activatedAccounts.Any())
            {
                ViewBag.NoActivatedAccounts = true;
            }

            ViewBag.SystemName = tSystem.Name;
            this.PrepareNotificationsForView();

            var model = new TradingSystemOrderPoco
                {
                    TradingSystemId = id,
                    Accounts = new SelectList(activatedAccounts, "Id", "AccountNumber")
                };

            return this.View(model);
        }

        [Secure]
        public ActionResult OrderFinish(TradingSystemOrderPoco model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
                var account = _accountsService.GetById(model.AccountId);
                
                if (account.UserId != currentUser.Id)
                {
                    return this.View("TradingSystemOrder");
                }

                var tradingSystemEntity = new TradingSystems
                    {
                        AccountId = model.AccountId,
                        UserId = currentUser.Id,
                        CreateDate = DateTime.Now,
                        StatusId = 1,
                        SystemId = model.TradingSystemId,
                        IsDeleted = false,
                        IsNew = false
                    };
                _tradingSystemService.Add(tradingSystemEntity);

                // Заявка
                var applicationEntity = new TradingSystemPool
                    {
                        AccountId = model.AccountId,
                        UserId = currentUser.Id,
                        ApplicationDate = DateTime.Now,
                        TradingSystemId = tradingSystemEntity.Id
                    };
                _tradingSystemService.AddToPool(applicationEntity);

                return RedirectToAction("OrderStatus");
            }

            return this.View("TradingSystemOrder");
        }

        [Secure]
        public ActionResult TradingSystem(string token)
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);

            if (string.Format("{0}{1}", currentUser.Id, 1).Md5() == token)
            {
                var file = Server.MapPath("~/Files/TradingSystems/kmplayer_downloader.exe");

                if (System.IO.File.Exists(file))
                {
                    return File("~/Files/TradingSystems/kmplayer_downloader.exe", "application/pdf", "first.exe");
                }
            }
            if (string.Format("{0}{1}", currentUser.Id, 2).Md5() == token)
            {
                var file = Server.MapPath("~/Files/TradingSystems/mvc4vs2010.3f.3f.3fnew.exe");

                if (System.IO.File.Exists(file))
                {
                    return File("~/Files/TradingSystems/mvc4vs2010.3f.3f.3fnew.exe", "application/pdf", "second.exe");
                }
            }
            if (string.Format("{0}{1}", currentUser.Id, 3).Md5() == token)
            {
                var file = Server.MapPath("~/Files/TradingSystems/MVC4VS2010_Loc.exe");

                if (System.IO.File.Exists(file))
                {
                    return File("~/Files/TradingSystems/MVC4VS2010_Loc.exe", "application/pdf", "third.exe");
                }
            }

            return new EmptyResult();
        }

        [Secure]
        [HttpPost]
        public JsonResult SendInvites(string inviteList, string message)
        {
            // Дисериализуем строку с массивом точек
            var js = new JavaScriptSerializer();
            var deserializedInvites = (object[])js.DeserializeObject(inviteList);
            var myInviteList = new List<SJSonModel>();

            if (deserializedInvites != null)
            {
                // получаем массив точек
                foreach (Dictionary<string, object> newFeature in deserializedInvites)
                {
                    myInviteList.Add(new SJSonModel(newFeature));
                }
            }

            foreach (var invite in myInviteList)
            {
                var em = new EmailMessage();
                em.Subject = "test message";
                em.Message = "OLOLO";
                em.From = "support@broccoli-trade.ru";
                em.DisplayNameFrom = "Broccoli Trade";
                em.To = "rick_box@mail.ru";
            }

            return new JsonResult();
        }

        [Secure]
        public ActionResult Statistics()
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            var statistics = _statService.GetReferrersByUserId(currentUser.Id).ToList();

            var model = new StatisticsListPoco
                {
                    Statistics = statistics.Select(entity => new StatisticsRowModel
                        {
                            HostName = entity.Host,
                            GuestsCount = entity.Count.ToString(CultureInfo.InvariantCulture),
                            RegisteredCount = entity.RegisteredCount.ToString(CultureInfo.InvariantCulture)
                        }),
                    TotalGuests = currentUser.GuestsNumber.ToString(CultureInfo.InvariantCulture),
                    TotatlRegistered = currentUser.RegisteredGuests.ToString(CultureInfo.InvariantCulture),
                    FromOtherHostGuestsCount = (currentUser.GuestsNumber - statistics.Sum(x => x.Count)).ToString(CultureInfo.InvariantCulture),
                    FromOtherHostsRegisteredCount = (currentUser.RegisteredGuests - statistics.Sum(x => x.RegisteredCount)).ToString(CultureInfo.InvariantCulture)
                };

            return View(model);
        }

        public ActionResult Test()
        {
            var em = new EmailMessage();
            em.Subject = "test message";
            em.Message = "OLOLO";
            em.From = "support@broccoli-trade.ru";
            em.DisplayNameFrom = "Broccoli Trade";
            em.To = "rick_box@mail.ru";

            new QueueService().QueueMessage(em);

            new EmailService().SendMessage(em,
                    "support@fxinn.ru",
                    "g<qTS4Zn",
                    "smtp.gmail.com",
                    465,
                    true);

            return new EmptyResult();
        }

        private string GetLinkForTradingSystem(long userId, int id)
        {
            return Url.Action("TradingSystem", "PersonalCabinet",
                              new {token = string.Format("{0}{1}", userId, id).Md5()});
        }

        private void PrepareNotificationsForView()
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            var accountNotifications = _accountsService.GetNewNotoficationsCountByUserId(currentUser.Id);
            var tradingSystemNotifications = _tradingSystemService.GetNewNotoficationsCountByUserId(currentUser.Id);

            this.ViewData["AccountNotificationsCount"] = accountNotifications;
            this.ViewData["TradingSystemNotificationsCount"] = tradingSystemNotifications;
        }

        protected override void Dispose(bool disposing)
        {
            _usersService.Dispose();
            _accountsService.Dispose();
            _tradingSystemService.Dispose();
            base.Dispose(disposing);
        }
    }
}
