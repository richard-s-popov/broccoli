using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroccoliTrade.Web.BroccoliMvc.Models.PersonalCabinet
{
    public class HostRowModel
    {
        public string HostName { get; set; }

        public string GuestsCount { get; set; }

        public string RegisteredCount { get; set; }

        public IEnumerable<DetailsHostRowModel> DetailsHosts { get; set; }  
    }
}