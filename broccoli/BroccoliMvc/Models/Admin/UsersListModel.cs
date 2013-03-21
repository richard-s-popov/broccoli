using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BroccoliTrade.Web.BroccoliMvc.Models.Account;

namespace BroccoliTrade.Web.BroccoliMvc.Models.Admin
{
    public class UsersListModel
    {
        public IEnumerable<UserProfilePoco> users { get; set; } 
    }
}