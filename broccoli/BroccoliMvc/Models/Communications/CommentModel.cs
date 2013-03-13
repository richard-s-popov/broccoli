using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroccoliTrade.Web.BroccoliMvc.Models.Communications
{
    public class CommentModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Body { get; set; }

        public DateTime Date { get; set; }
    }
}