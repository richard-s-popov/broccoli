using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroccoliTrade.Web.BroccoliMvc.Models.PersonalCabinet
{
    public class StatisticsListPoco
    {
        public IEnumerable<HostRowModel> StatisticsByHost { get; set; }

        public IList<IDictionary<string,string>> StatisticsGuestsByDate { get; set; }

        public IList<IDictionary<string, string>> StatisticsRegisteredByDate { get; set; }

        public string FromOtherHostGuestsCount { get; set; }

        public string FromOtherHostsRegisteredCount { get; set; }

        public string TotalGuests { get; set; }

        public string TotatlRegistered { get; set; }

        public List<TradingSystem> SuccessSystem { get; set; } 
    }

    public class TradingSystem
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}