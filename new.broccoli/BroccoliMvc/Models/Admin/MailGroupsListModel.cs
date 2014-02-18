using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroccoliTrade.Web.BroccoliMvc.Models.Admin
{
    public class MailGroupsListModel
    {
        public IEnumerable<MailGroup> GroupList { get; set; } 
    }

    public class MailGroup
    {
        public int Id { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<Mail> MailList { get; set; } 
    }

    public class Mail
    {
        public int Id { get; set; }

        public int MailNumber { get; set; }

        public string MailName { get; set; }

        public string MailBody { get; set; }
    }
}