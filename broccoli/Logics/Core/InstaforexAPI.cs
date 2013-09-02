using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;

namespace BroccoliTrade.Logics.Core
{
    public static class InstaforexAPI
    {
        public static void TestMethod()
        {
            const int login = 554191;
            const string pass = "broccoli";
            var token = GetToken(login, pass);
            var result = GetCommissionRecords(login, token);
            //foreach (var trade in result)
            //{
            //    Console.WriteLine("{0} {1}", trade.Partner, trade.BalanceRecords.Count);
            //}
        }

        public static string GetToken(int login, string password)
        {
            var client = new HttpClient();
            HttpContent tokenContent = new ObjectContent(typeof(AccessTokenRequest), new AccessTokenRequest { Login = login, Password = password }, new JsonMediaTypeFormatter());
            var token = client.PostAsync("http://client-api.instaforex.com/api/Authentication/RequestPartnerApiToken", tokenContent).Result.Content.ReadAsStringAsync().Result;
            return token;
        }

        public static bool GetCommissionRecords(int login, string token){
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("passkey", token);
            var result = client.GetAsync(string.Format("http://client-api.instaforex.com/partner/GetCommissionRecords/{0}", login)).Result;
            return true;
        }

        public class AccessTokenRequest
        {
            public int Login { get; set; }
            public string Password { get; set; }
        }
    }
}
