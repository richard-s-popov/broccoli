using System;
using System.Linq;
using System.Web.Mvc;
using BroccoliTrade.Domain;
using BroccoliTrade.Domain.Models;
using BroccoliTrade.Logics.Infrastructure.Extensions;
using BroccoliTrade.Logics.Interfaces.Account;
using BroccoliTrade.Logics.Interfaces.Membership;
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

        public PersonalCabinetController(
            IUsersService usersService,
            IAccountsService accountsService,
            ITradingSystemService tradingSystemService)
        {
            _usersService = usersService;
            _accountsService = accountsService;
            _tradingSystemService = tradingSystemService;
        }

        [Secure]
        public ActionResult Index()
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);

            this.ViewBag.User = currentUser;

            return View("PersonalCabinet");
        }

        [Secure]
        public ActionResult ActivateAccount()
        {
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
                    IsDeleted = false
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

            var model = new AccountListPoco
                {
                    accounts = accounts.Select(entity => new AccountRowModel
                        {
                            Number = entity.AccountNumber,
                            Status = entity.Status.Name
                        })
                };

            ViewBag.NoData = !accounts.Any();

            return this.View(model);
        }

        [Secure]
        public JsonResult DeleteAccount(string account)
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            var currentAccount = _accountsService.GetByAccountNumber(account);
            
            if (currentAccount.UserId == currentUser.Id)
            {
                _accountsService.DeleteAccount(account);

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
                        IsDeleted = false
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

            return new EmptyResult();
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

        protected override void Dispose(bool disposing)
        {
            _usersService.Dispose();
            _accountsService.Dispose();
            _tradingSystemService.Dispose();
            base.Dispose(disposing);
        }
    }
}
