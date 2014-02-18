using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BroccoliTrade.Logics.Core
{
    public static class InstaforexAPI
    {
        public static List<int> GetAccounts(){
            var myHttpWebRequest = (HttpWebRequest)WebRequest
                    .Create(string.Format("http://client-api.instaforex.com/partner/GetClickStatistics/{0}", 554191));

            var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            var myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.UTF8);

            var response = myStreamReader.ReadToEnd();
            dynamic json = JToken.Parse(response);

            var list = new List<int>();

            list.AddRange(json[0].ReferalAccounts.ToObject<List<int>>());
            list.AddRange(json[1].ReferalAccounts.ToObject<List<int>>());
            list.AddRange(json[2].ReferalAccounts.ToObject<List<int>>());

            return list;
        }
    }
}
