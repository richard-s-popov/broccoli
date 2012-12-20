using System.Web.Mvc;
using System.Web.Routing;

namespace BroccoliTrade.Web.BroccoliMvc.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "RegisterForm", // Route name
                "register", // URL with parameters
                new { controller = "Account", action = "RegisterForm" } // Parameter defaults
            );

            routes.MapRoute(
                "PersonalCabinet", // Route name
                "cabinet", // URL with parameters
                new { controller = "PersonalCabinet", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "ActivateAccount", // Route name
                "activate", // URL with parameters
                new { controller = "PersonalCabinet", action = "ActivateAccount" } // Parameter defaults
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}