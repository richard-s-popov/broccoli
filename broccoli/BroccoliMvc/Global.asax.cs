using System;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BroccoliTrade.Logics.Core;
using BroccoliTrade.Logics.Impl.Account;
using BroccoliTrade.Logics.Impl.Communications;
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

            var thread = new Thread(FunctionCheckTimer)
                {
                    IsBackground = true, 
                    Name = "FunctionCheckTimer"
                };
            thread.Start();

            var compilationThread = new Thread(FunctionCompileTimer)
                {
                    IsBackground = true,
                    Name = "FunctionCompileTimer"
                };
            compilationThread.Start();

            var mailSenderThread = new Thread(FunctionMailSernderTimer)
                {
                    IsBackground = true,
                    Name = "FunctionMailSernderTimer"
                };
            mailSenderThread.Start();

            var accountCountWorkerThread = new Thread(FunctionAccountCountWorkerTimer)
            {
                IsBackground = true,
                Name = "FunctionAccountCountWorkerTimer"
            };
            accountCountWorkerThread.Start();
        }

        protected void FunctionCheckTimer()
        {
            var timer = new System.Timers.Timer();
            timer.Elapsed += TimerEvent;
            timer.Interval = 60000; // 1 минут
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();
        }

        protected void FunctionCompileTimer()
        {
            var timer = new System.Timers.Timer();
            timer.Elapsed += CompileTimerEvent;
            timer.Interval = 60000; // 1 минут
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();
        }

        protected void FunctionMailSernderTimer()
        {
            var timer = new System.Timers.Timer();
            timer.Elapsed += MailSenderTimerEvent;
            timer.Interval = 3600000; // 1 час
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();
        }

        protected void FunctionAccountCountWorkerTimer()
        {
            var timer = new System.Timers.Timer();
            timer.Elapsed += AccountCountWorkerTimerEvent;
            timer.Interval = 3600000; // 1 час
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();
        }

        protected void TimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            new AccountsService().CheckAccountsInPool();
            new TradingSystemService().CheckTradingSystemInPool();
        }

        protected void CompileTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            
        }

        protected void MailSenderTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == 13)
            {
                var service = new CommunicationService();

                switch (service.GetMailDay())
                {
                    case "5":
                        service.SetMailDay(1);
                        break;
                    case "4":
                        service.SetMailDay(5);
                        break;
                    case "3":
                        service.RunSendMails();
                        service.SetMailDay(4);
                        break;
                    case "2":
                        service.SetMailDay(3);
                        break;
                    case "1":
                        service.RunSendMails();
                        service.SetMailDay(2);
                        break;
                }
            }
        }

        protected void AccountCountWorkerTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            new AccountsService().AccountCountWorker();
        }
    }
}