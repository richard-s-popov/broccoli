using System.Web;
using System.Web.Mvc;

namespace BroccoliTrade.Web.BroccoliMvc.Infrastructure.Attributes
{
    public class SecureAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

            }
            else
            {
                var rres = new RedirectResult("~/login");
                filterContext.Result = rres;
            }
        }
    }
}