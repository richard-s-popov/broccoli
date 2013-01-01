using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BroccoliTrade.Logics.Interfaces.Membership;

namespace BroccoliTrade.Web.BroccoliMvc.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IUsersService _usersService;

        public HomeController(
            IUsersService usersService)
        {
            _usersService = usersService;
        }

        public ActionResult Index(string token)
        {
            var ownerUser = _usersService.GetUserByLogin(token);
            
            if (ownerUser != null)
            {
                if (!ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("owner"))
                {
                    var cookie = new HttpCookie("owner");
                    cookie.Value = ownerUser.Id.ToString(CultureInfo.InvariantCulture);
                    cookie.Expires = DateTime.Now.AddHours(1);
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                    if (ownerUser.GuestsNumber == null)
                    {
                        ownerUser.GuestsNumber = 1;
                    }
                    else
                    {
                        ownerUser.GuestsNumber++;
                    }
                    _usersService.SaveChanges();
                }
            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _usersService.Dispose();
            base.Dispose(disposing);
        }
    }
}
