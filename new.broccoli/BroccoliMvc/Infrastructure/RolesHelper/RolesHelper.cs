using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using BroccoliTrade.Domain;
using BroccoliTrade.Logics.Interfaces.Membership;

namespace BroccoliTrade.Web.BroccoliMvc.Infrastructure.RolesHelper
{
    public static class RolesHelper
    {
        public static bool IsUserInRole(this IPrincipal user, String role)
        {
            var currentUser = new BroccoliEntities().Users.FirstOrDefault(x => x.Email == user.Identity.Name);

            if (currentUser == null)
            {
                return false;
            }

            return currentUser.Roles.RoleName == role;
        }
    }
}