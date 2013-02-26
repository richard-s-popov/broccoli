using System.Web.Mvc;
using BroccoliTrade.Logics.Impl.Account;
using BroccoliTrade.Logics.Impl.Communications;
using BroccoliTrade.Logics.Impl.Membership;
using BroccoliTrade.Logics.Impl.Statistics;
using BroccoliTrade.Logics.Impl.TradingSystem;
using BroccoliTrade.Logics.Interfaces.Account;
using BroccoliTrade.Logics.Interfaces.Communications;
using BroccoliTrade.Logics.Interfaces.Membership;
using BroccoliTrade.Logics.Interfaces.Statistics;
using BroccoliTrade.Logics.Interfaces.TradingSystem;
using Microsoft.Practices.Unity;
using Unity.Mvc3;

namespace BroccoliTrade.Web.BroccoliMvc.Infrastructure.Unity
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IUsersService, UsersService>();
            container.RegisterType<IMembershipService, MembershipService>();
            container.RegisterType<IAccountsService, AccountsService>();
            container.RegisterType<ITradingSystemService, TradingSystemService>();
            container.RegisterType<IStatService, StatService>();
            container.RegisterType<ICommunicationService, CommunicationService>();

            return container;
        }
    }
}