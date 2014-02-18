using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BroccoliTrade.Domain;

namespace BroccoliTrade.Web.BroccoliMvc.Infrastructure.UserHelper
{
    public class UserHelper
    {
        public static string GetUserName()
        {
            var currentUser = new BroccoliEntities().Users.FirstOrDefault(x => x.Email == HttpContext.Current.User.Identity.Name);

            return currentUser == null ? string.Empty : currentUser.Name;
        }
    }
}