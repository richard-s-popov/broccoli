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
                "ActivateAccount", // Route name
                "activate", // URL with parameters
                new { controller = "PersonalCabinet", action = "ActivateAccount" } // Parameter defaults
            );

            routes.MapRoute(
                "Login", // Route name
                "login", // URL with parameters
                new { controller = "Account", action = "LogOn" } // Parameter defaults
            );

            routes.MapRoute(
                "Contact", // Route name
                "contact", // URL with parameters
                new { controller = "Home", action = "ContactUs" } // Parameter defaults
            );

            routes.MapRoute(
                "BroccoliOffer", // Route name
                "offer", // URL with parameters
                new { controller = "Home", action = "BroccoliOffer" } // Parameter defaults
            );

            routes.MapRoute(
                "BrokerInfo", // Route name
                "forexinn", // URL with parameters
                new { controller = "Home", action = "BrokerInfo" } // Parameter defaults
            );

            routes.MapRoute(
                "VPS", // Route name
                "vps", // URL with parameters
                new { controller = "Home", action = "VPS" } // Parameter defaults
            );

            routes.MapRoute(
                "Comments", // Route name
                "comments", // URL with parameters
                new { controller = "Home", action = "Comments" } // Parameter defaults
            );

            routes.MapRoute(
                "Partners", // Route name
                "partners", // URL with parameters
                new { controller = "Home", action = "Partners" } // Parameter defaults
            );

            routes.MapRoute(
                "Partner", // Route name
                "partner/{id}", // URL with parameters
                new { controller = "Home", action = "Partner", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}