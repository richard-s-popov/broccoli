using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroccoliTrade.Web.BroccoliMvc.Models.PersonalCabinet
{
    public class TradingSystemListPoco
    {
        public IEnumerable<TradingSystemRowModel> TradingSystems { get; set; } 
    }
}