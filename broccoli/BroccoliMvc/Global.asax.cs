using System;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BroccoliTrade.Logics.Impl.Account;
using BroccoliTrade.Logics.Impl.TradingSystem;
using BroccoliTrade.Logics.Interfaces.Account;
using BroccoliTrade.Web.BroccoliMvc.App_Start;
using BroccoliTrade.Web.BroccoliMvc.Infrastructure.Unity;

namespace BroccoliTrade.Web.BroccoliMvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Bootstrapper.Initialise();

            Thread thread = new Thread(new ThreadStart(Function));
            thread.IsBackground = true;
            thread.Name = "Function";
            thread.Start();
        }

        protected void Function()
        {
            var timer = new System.Timers.Timer();
            timer.Elapsed += TimerEvent;
            timer.Interval = 60000; // 5 минут
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();
        }

        protected void TimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            new AccountsService().CheckAccountsInPool();
            new TradingSystemService().CheckTradingSystemInPool();
        }
    }
}