using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BroccoliTrade.Domain;
using BroccoliTrade.Logics.Interfaces.Communications;
using BroccoliTrade.Logics.Interfaces.Membership;

namespace BroccoliTrade.Web.BroccoliMvc.Infrastructure.Attributes
{
    public class RoleAttribute : ActionFilterAttribute
    {
        public string Role { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentUser = new BroccoliEntities().Users.FirstOrDefault(x => x.Email == HttpContext.Current.User.Identity.Name);

            if (currentUser == null || currentUser.Roles.RoleName != Role)
            {
                filterContext.Result = new RedirectResult("~/");
            }
        }
    }
}