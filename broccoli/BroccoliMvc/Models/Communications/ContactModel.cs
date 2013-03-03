using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroccoliTrade.Web.BroccoliMvc.Models.Communications
{
    public class ContactModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}