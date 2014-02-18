using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BroccoliTrade.Web.BroccoliMvc.Models.PersonalCabinet
{
    public class TradingSystemOrderPoco
    {
        public IEnumerable<SelectListItem> Accounts { get; set; }

        public int? AccountId { get; set; }

        [Required]
        public int TradingSystemId { get; set; }
    }
}