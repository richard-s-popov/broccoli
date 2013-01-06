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
            var ownerUser = _usersService.GetUserByLogin(token);
            
            if (ownerUser != null)
            {
                if (Request.Cookies["owner"] == null)
                {
                    var cookie = new HttpCookie("owner");
                    cookie.Value = ownerUser.Id.ToString(CultureInfo.InvariantCulture);
                    cookie.Expires = DateTime.Now.AddHours(1);
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                    ownerUser.GuestsNumber++;
                    _usersService.SaveChanges();

                    // Если есть реферальный url, то сохраняем в статистику
                    if (HttpContext.Request.UrlReferrer != null)
                    {
                        var urlReferrer = HttpContext.Request.UrlReferrer;

                        // Создаем новые cookie
                        if (Request.Cookies["refHost"] == null)
                        {
                            var refHostCookie = new HttpCookie("refHost");
                            refHostCookie.Value = urlReferrer.Host.ToString(CultureInfo.InvariantCulture);
                            refHostCookie.Expires = DateTime.Now.AddHours(1);
                            this.ControllerContext.HttpContext.Response.Cookies.Add(refHostCookie);
                        }
                        else // или обновляем имеющиеся
                        {
                            Response.Cookies["refHost"].Value = urlReferrer.Host;
                        }

                        var referrer = _statService.GetReferrerByUserAndHost(ownerUser.Id, urlReferrer.Host);
                        
                        if (referrer == null)
                        {
                            var entity = new Referrer
                                {
                                    Host = urlReferrer.Host,
                                    Count = 1,
                                    IsDeleted = false,
                                    OwnerId = ownerUser.Id
                                };

                            _statService.AddReferrer(entity);
                        }
                        else
                        {
                            referrer.Count++;
                            _statService.Save();
                        }
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

        protected override void Dispose(bool disposing)
        {
            _usersService.Dispose();
            _statService.Dispose();
            base.Dispose(disposing);
        }
    }
}
