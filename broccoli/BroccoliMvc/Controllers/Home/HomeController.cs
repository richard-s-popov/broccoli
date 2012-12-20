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

        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _usersService.Dispose();
            base.Dispose(disposing);
        }
    }
}
