using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BroccoliTrade.Domain;
using BroccoliTrade.Logics.Interfaces.Membership;
using BroccoliTrade.Logics.Interfaces.Statistics;

namespace BroccoliTrade.Web.BroccoliMvc.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IStatService _statService;

        public HomeController(
            IUsersService usersService,
            IStatService statService)
        {
            _usersService = usersService;
            _statService = statService;
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

        public ActionResult BrokerInfo()
        {
            return View();
        }

        public ActionResult BroccoliOffer()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _usersService.Dispose();
            _statService.Dispose();
            base.Dispose(disposing);
        }
    }
}
