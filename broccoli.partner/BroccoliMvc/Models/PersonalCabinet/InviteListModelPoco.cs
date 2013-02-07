using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroccoliTrade.Web.BroccoliMvc.Models.PersonalCabinet
{
    public class SJSonModel
    {
        public SJSonModel(Dictionary<string, object> inviteList)
        {
            if (inviteList.ContainsKey("name"))
            {
                Name = (string)inviteList["name"];
            }
            if (inviteList.ContainsKey("email"))
            {
                Email = (string)inviteList["email"];
            }
        }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}