using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroccoliTrade.Web.BroccoliMvc.Models.PersonalCabinet
{
    public class AccountListPoco
    {
        public IEnumerable<AccountRowModel> accounts { get; set; } 
    }
}