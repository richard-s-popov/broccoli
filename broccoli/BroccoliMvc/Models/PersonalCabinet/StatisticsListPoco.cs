using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroccoliTrade.Web.BroccoliMvc.Models.PersonalCabinet
{
    public class StatisticsListPoco
    {
        public IList<HostRowModel> Statistics { get; set; }

        public string FromOtherHostGuestsCount { get; set; }

        public string FromOtherHostsRegisteredCount { get; set; }

        public string TotalGuests { get; set; }

        public string TotatlRegistered { get; set; }
    }
}