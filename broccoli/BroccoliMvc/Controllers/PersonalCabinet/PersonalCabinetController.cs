using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Configuration;
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
        public ActionResult TradingSystemOrder(int id, string errorMessage)
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

            ViewBag.SystemName = tSystem.Name;

            if (!activatedAccounts.Any())
            {
                ViewBag.NoActivatedAccounts = true;
            }

            this.PrepareNotificationsForView();

            var model = new TradingSystemOrderPoco
                {
                    TradingSystemId = id,
                    Accounts = new SelectList(activatedAccounts, "Id", "AccountNumber")
                };

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
                return View(model);
            }

            return this.View(model);
        }

        [Secure]
        public ActionResult OrderFinish(TradingSystemOrderPoco model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);

                if (!_accountsService.GetAccountsByUserId(currentUser.Id).Any())
                {
                    return RedirectToAction("TradingSystemOrder", new { @id = model.TradingSystemId, @errorMessage = "У вас еще нет активированных счетов. Пожалуйста, активируйте снача счет, следуя инструкциям на главной странице." });
                }

                if (model.AccountId == null)
                {
                    return RedirectToAction("TradingSystemOrder", new {@id = model.TradingSystemId, @errorMessage = "Выберите счет"});
                }

                var account = _accountsService.GetById(model.AccountId.Value);
                var statistics = _statService.GetReferrersByUserId(currentUser.Id);
                var tradingSystemKey = string.Empty;

                switch (model.TradingSystemId)
                {
                    case 2:
                        tradingSystemKey = "FirstSystem";
                        break;
                    case 3:
                        tradingSystemKey = "SecondSystem";
                        break;
                    default:
                        tradingSystemKey = "OtherSystem";
                        break;
                }

                var countForFirst = Convert.ToInt32(ConfigurationManager.AppSettings[tradingSystemKey]);
                
                if (account.UserId != currentUser.Id)
                {
                    return this.View("TradingSystemOrder");
                }

                var tradingSystemEntity = new TradingSystems
                    {
                        AccountId = model.AccountId.Value,
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
                        AccountId = model.AccountId.Value,
                        UserId = currentUser.Id,
                        ApplicationDate = DateTime.Now,
                        TradingSystemId = tradingSystemEntity.Id,
                        ByInvite = statistics.Count(x => x.Registered) >= countForFirst
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

        public ActionResult GetInstruction(string tradingSystem)
        {
            if (tradingSystem == "Money+")
            {
                var file = Server.MapPath("~/Files/TradingSystems/Instructions/Money+.docx");

                if (System.IO.File.Exists(file))
                {
                    return File("~/Files/TradingSystems/Instructions/Money+.docx", "application/pdf", "Money+.docx");
                }
            }
            if (tradingSystem == "Garanted")
            {
                var file = Server.MapPath("~/Files/TradingSystems/Instructions/GarantedProfit.docx");

                if (System.IO.File.Exists(file))
                {
                    return File("~/Files/TradingSystems/Instructions/GarantedProfit.docx", "application/pdf", "GarantedProfit.docx");
                }
            }
            if (tradingSystem == "MaxTrade")
            {
                var file = Server.MapPath("~/Files/TradingSystems/Instructions/Max Trade.docx");

                if (System.IO.File.Exists(file))
                {
                    return File("~/Files/TradingSystems/Instructions/Max Trade.docx", "application/pdf", "Max Trade.docx");
                }
            }

            return new EmptyResult();
        }

        [Secure]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SendInvites(string inviteList, string message, string subject)
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);

            // Дисериализуем строку с массивом точек
            var js = new JavaScriptSerializer();
            var deserializedInvites = (object[])js.DeserializeObject(inviteList);
            var myInviteList = new List<SJSonModel>();

            if (deserializedInvites != null)
            {
                // получаем массив точек
                myInviteList.AddRange(from Dictionary<string, object> newFeature in deserializedInvites select new SJSonModel(newFeature));
            }

            foreach (var invite in myInviteList)
            {
                var em = new EmailMessage
                    {
                        Subject = subject,
                        Message = message.Replace("%ИМЯ%", invite.Name),
                        From = "support@broccoli-trade.ru",
                        DisplayNameFrom = currentUser.Name,
                        To = invite.Email
                    };

                new QueueService().QueueMessage(em);
            }

            return new JsonResult();
        }

        [Secure]
        public ActionResult Statistics(string period, DateTime? start, DateTime? end)
        {
            var currentUser = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            IEnumerable<Referrer> statistics;
            DateTime dateFrom;
            DateTime dateTo;

            if (start == null && end == null || start > end)
            {
                switch (period)
                {
                    case "today":
                        statistics = _statService.GetReferrersByUserIdAndPeriod(
                            currentUser.Id, 
                            DateTime.Today, 
                            DateTime.Now).ToList();
                        ViewBag.Period = "today";
                        dateFrom = DateTime.Today;
                        dateTo = DateTime.Now;
                        break;

                    case "yesterday":
                        statistics = _statService.GetReferrersByUserIdAndPeriod(
                            currentUser.Id, 
                            DateTime.Today.AddDays(-1), 
                            DateTime.Today).ToList();
                        ViewBag.Period = "yesterday";
                        dateFrom = DateTime.Today.AddDays(-1);
                        dateTo = DateTime.Today;
                        break;

                    case "week":
                        statistics = _statService.GetReferrersByUserIdAndPeriod(
                            currentUser.Id, 
                            DateTime.Now.AddDays(-7), 
                            DateTime.Now).ToList();
                        ViewBag.Period = "week";
                        dateFrom = DateTime.Now.AddDays(-7);
                        dateTo = DateTime.Now;
                        break;

                    case "month":
                        statistics = _statService.GetReferrersByUserIdAndPeriod(
                            currentUser.Id, 
                            DateTime.Now.AddMonths(-1), 
                            DateTime.Now).ToList();
                        ViewBag.Period = "month";
                        dateFrom = DateTime.Now.AddMonths(-1);
                        dateTo = DateTime.Now;
                        break;

                    default:
                        ViewBag.Period = "allTime";
                        statistics = _statService.GetReferrersByUserId(currentUser.Id).ToList();

                        if (statistics.Any())
                        {
                            dateFrom = statistics.Min(x => x.Date);
                            dateTo = statistics.Max(x => x.Date);
                        }
                        else
                        {
                            dateFrom = DateTime.Now;
                            dateTo = DateTime.Now;
                        }

                        break;
                }

                ViewBag.DateFrom = string.Empty;
                ViewBag.DateTo = string.Empty;
            }
            else
            {
                statistics = _statService.GetReferrersByUserIdAndPeriod(currentUser.Id, start, end).ToList();
                
                ViewBag.Period = "datePeriod";
                ViewBag.DateFrom = start.GetValueOrDefault().ToShortDateString();
                ViewBag.DateTo = end.GetValueOrDefault().ToShortDateString();

                dateFrom = (DateTime) start;
                dateTo = (DateTime) end;
            }

            var model = new StatisticsListPoco
                {
                    StatisticsByHost = statistics.GroupBy(x => x.Host).Select(group => new HostRowModel
                        {
                            HostName = @group.Key == "undefined" ? "прочие*" : @group.Key,
                            GuestsCount = @group.Count(x => !x.Registered).ToString(),
                            RegisteredCount = @group.Count(x => x.Registered).ToString(),
                            DetailsHosts =
                                @group.Key == "undefined"
                                    ? new List<DetailsHostRowModel>()
                                    : @group.GroupBy(x => x.FullReferrerUrl).Select(x => new DetailsHostRowModel
                                        {
                                            FullReferrerUrl = x.Key,
                                            GuestsCount = x.Count(r => !r.Registered).ToString(),
                                            RegisteredCount = x.Count(r => r.Registered).ToString()
                                        }).OrderByDescending(s => s.GuestsCount).ToList()
                        }),
                    TotalGuests = statistics.Count(x => !x.Registered).ToString(),
                    TotatlRegistered = statistics.Count(x => x.Registered).ToString(),
                    StatisticsGuestsByDate = new List<IDictionary<string,string>>(),
                    StatisticsRegisteredByDate = new List<IDictionary<string, string>>()
                };

            if (statistics.Any())
            {
                for (var date = dateFrom.Subtract(dateFrom.TimeOfDay); date <= dateTo; date = date.AddDays(1))
                {
                    var dicGuests = new Dictionary<string, string> {{"Date", ToJSGenDate(date)}};
                    var dicReg = new Dictionary<string, string> {{"Date", ToJSGenDate(date)}};

                    foreach (var byHost in statistics.Where(x => x.Date.Date == date).GroupBy(group => group.Host))
                    {
                        dicGuests.Add(byHost.Key == "undefined" ? "прочие*" : byHost.Key,
                                      byHost.Count(x => !x.Registered).ToString());
                        dicReg.Add(byHost.Key == "undefined" ? "прочие*" : byHost.Key,
                                   byHost.Count(x => x.Registered).ToString());
                    }

                    foreach (var byHost in statistics.GroupBy(group => group.Host))
                    {
                        if (!dicGuests.ContainsKey(byHost.Key == "undefined" ? "прочие*" : byHost.Key))
                        {
                            dicGuests.Add(byHost.Key == "undefined" ? "прочие*" : byHost.Key, "0");
                        }
                        if (!dicReg.ContainsKey(byHost.Key == "undefined" ? "прочие*" : byHost.Key))
                        {
                            dicReg.Add(byHost.Key == "undefined" ? "прочие*" : byHost.Key, "0");
                        }
                    }

                    model.StatisticsGuestsByDate.Add(dicGuests);
                    model.StatisticsRegisteredByDate.Add(dicReg);
                }
            }

            if (statistics.Count(x => x.Registered) > Convert.ToInt32(ConfigurationManager.AppSettings["FirstSystem"]))
            {
                ViewBag.SystemSuccess = true;
                model.SuccessSystem = new List<TradingSystem>();

                if (statistics.Count(x => x.Registered) > Convert.ToInt32(ConfigurationManager.AppSettings["SecondSystem"]))
                {
                    model.SuccessSystem.Add(new TradingSystem
                        {
                            Id = 2,
                            Name = _tradingSystemService.GetSystemById(2).Name
                        });
                    model.SuccessSystem.Add(new TradingSystem
                        {
                            Id = 3,
                            Name = _tradingSystemService.GetSystemById(3).Name
                        });
                }
                else
                {
                    model.SuccessSystem.Add(new TradingSystem
                        {
                            Id = 2,
                            Name = _tradingSystemService.GetSystemById(2).Name
                        });
                }
            }
            else
            {
                ViewBag.SystemSuccess = false;
            }

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
                    "support@broccoli-trade.ru",
                    "gqTS4Zne",
                    "smtp.yandex.ru",
                    587,
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

        private string ToJSGenDate(DateTime date)
        {
            return string.Format("new Date({0}, {1}, {2})", date.Year, date.Month - 1, date.Day);
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
