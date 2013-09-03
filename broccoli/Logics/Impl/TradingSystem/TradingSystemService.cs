using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using BroccoliTrade.Domain;
using BroccoliTrade.Domain.Models;
using BroccoliTrade.Logics.Infrastructure.Certificates;
using BroccoliTrade.Logics.Interfaces.TradingSystem;
using BroccoliTrade.Logics.MSMQ;
using Newtonsoft.Json.Linq;

namespace BroccoliTrade.Logics.Impl.TradingSystem
{
    public class TradingSystemService : ITradingSystemService
    {
        public BroccoliEntities db = null;

        public TradingSystemService()
        {
            db = new BroccoliEntities();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public TradingSystems GetById(int id)
        {
            return db.TradingSystems
                .Include("Systems")
                .Include("Status")
                .Include("Accounts")
                .FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        }

        public Systems GetSystemById(int id)
        {
            return db.Systems.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<TradingSystems> GetTradingSystemsByUserId(long id)
        {
            return db.TradingSystems
                .Include("Systems")
                .Include("Status")
                .Include("Accounts")
                .Where(x => x.UserId == id && !x.IsDeleted);
        }

        public int GetNewNotoficationsCountByUserId(long userId)
        {
            return db.TradingSystems.Count(x => x.IsNew && x.UserId == userId && !x.IsDeleted);
        }

        public void Add(TradingSystems entity)
        {
            db.TradingSystems.Add(entity);
            db.SaveChanges();
        }

        public void AddWithoutSaving(TradingSystems entity)
        {
            db.TradingSystems.Add(entity);
        }

        public void AddToPool(TradingSystemPool entity)
        {
            db.TradingSystemPool.Add(entity);
            db.SaveChanges();
        }

        public void Delete(TradingSystems entity)
        {
            var tSystem = db.TradingSystems.FirstOrDefault(x => x.Id == entity.Id);

            if (tSystem != null)
            {
                tSystem.IsDeleted = true;
            }

            db.SaveChanges();
        }

        public void Save()
        {
            db.SaveChanges();
        }
        
        public void CheckTradingSystemInPool()
        {
            var time = DateTime.Now.AddMinutes(-1 * GetRandomMinutes(5));
            var accounts = db.TradingSystemPool.Where(x => x.ApplicationDate < time);

            ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

            foreach (var tradingSystemPool in accounts)
            {
                var myHttpWebRequest = (HttpWebRequest)WebRequest
                    .Create(string.Format("https://my.forexinn.ru/agent-api/check-involved-account/broccoli/jcmbogje9b5uxs/{0}", tradingSystemPool.Accounts.AccountNumber));

                var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                var myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.UTF8);

                var response = myStreamReader.ReadToEnd();
                dynamic json = JToken.Parse(response);

                var tradingSystem = this.GetById(tradingSystemPool.TradingSystemId);

                if (((bool)json["result"] && this.CanTakeTradingSystem(tradingSystem.Systems.Id, (int)json["count"])) || tradingSystemPool.ByInvite)
                {
                    tradingSystem.StatusId = 4;
                    tradingSystem.IsNew = true;

                    EmailMessage em;

                    if (this.Compile(tradingSystem.Accounts.AccountNumber, tradingSystem.SystemId, tradingSystem.Users))
                    {
                        tradingSystem.StatusId = 2;

                        em = new EmailMessage
                        {
                            Subject = string.Format("{0} активирован", tradingSystem.Systems.Name),
                            Message = string.Format("Здравствуйте, {0}. {1} активирован и доступен для загрузки.", tradingSystem.Users.Name, tradingSystem.Systems.Name),
                            From = "support@broccoli-trade.ru",
                            DisplayNameFrom = "Broccoli Trade",
                            To = tradingSystem.Users.Email
                        };
                    }
                    else
                    {
                        tradingSystem.StatusId = 5;

                        em = new EmailMessage
                        {
                            Subject = string.Format("Произошла ошибка активации {0}", tradingSystem.Systems.Name),
                            Message = string.Format("Здравствуйте, {0}. {1} не активирован, произошла ошибка, обратитесь в службу поддержки.", tradingSystem.Users.Name, tradingSystem.Systems.Name),
                            From = "support@broccoli-trade.ru",
                            DisplayNameFrom = "Broccoli Trade",
                            To = tradingSystem.Users.Email
                        };
                    }

                    new QueueService().QueueMessage(em);
                }
                else
                {
                    tradingSystem.StatusId = 3;
                    tradingSystem.IsNew = true;

                    var em = new EmailMessage
                    {
                        Subject = string.Format("Заявка на {0} отклонена", tradingSystem.Systems.Name),
                        Message = string.Format("Здравствуйте, {0}. Заявка на {1} отклонена.", tradingSystem.Users.Name, tradingSystem.Systems.Name),
                        From = "support@broccoli-trade.ru",
                        DisplayNameFrom = "Broccoli Trade",
                        To = tradingSystem.Users.Email
                    };

                    new QueueService().QueueMessage(em);
                }

                db.TradingSystemPool.Remove(tradingSystemPool);
            }

            db.SaveChanges();
        }

        public string GetCompiledPath(int systemId, Users user)
        {
            if (systemId == 1)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "TradingSystems", "Compile",
                                    "Money+eurusd_" + user.Id + ".ex4");
            }

            if (systemId == 2)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "TradingSystems", "Compile",
                                    "GarantedProfit_" + user.Id + ".ex4");
            }

            if (systemId == 3)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "TradingSystems", "Compile",
                                    "MaxTrade_" + user.Id + ".ex4");
            }

            return null;
        }

        private bool CanTakeTradingSystem(int systemId, int count)
        {
            if (systemId == 3 && count > 1000)
            {
                return true;
            }
            if (systemId == 2 && count > 100)
            {
                return true;
            }
            if (systemId == 1)
            {
                return true;
            }

            return false;
        }

        private int GetRandomMinutes(int range)
        {
            var random = new Random();

            return random.Next(range);
        }

        private bool Compile(string account, int systemId, Users user)
        {
            this.DeleteOldFilesFromCompileDir(systemId, user);
            File.Copy(this.GetTemplatePath(systemId), this.GetCompilePath(systemId, user));

            var file = File.ReadAllText(this.GetCompilePath(systemId, user));
            file = file.Replace("##account_number", account);
            File.WriteAllText(this.GetCompilePath(systemId, user), file);

            Process.Start(@"C:\Program Files (x86)\MetaTrader ForexInn Ltd\metalang.exe",
                          this.GetCompilePath(systemId, user));

            while (!File.Exists(this.GetCompilePath(systemId, user).Replace(".mq4", string.Empty) + ".ex4"))
            {
                if (File.Exists(this.GetCompilePath(systemId, user).Replace(".mq4", string.Empty) + ".log"))
                    return false;
            }

            return true;
        }

        private void DeleteOldFilesFromCompileDir(int systemId, Users user)
        {
            if (File.Exists(this.GetCompilePath(systemId, user).Replace(".mq4", string.Empty) + ".ex4"))
            {
                File.Delete(this.GetCompilePath(systemId, user).Replace(".mq4", string.Empty) + ".ex4");
            }

            if (File.Exists(this.GetCompilePath(systemId, user)))
            {
                File.Delete(this.GetCompilePath(systemId, user));
            }

            if (File.Exists(this.GetCompilePath(systemId, user).Replace(".mq4", string.Empty) + ".log"))
            {
                File.Delete(this.GetCompilePath(systemId, user).Replace(".mq4", string.Empty) + ".log");
            }
        }

        private string GetCompilePath(int systemId, Users user)
        {
            if (systemId == 1)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "TradingSystems", "Compile",
                                    "Money+eurusd_" + user.Id + ".mq4");
            }

            if (systemId == 2)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "TradingSystems", "Compile",
                                    "GarantedProfit_" + user.Id + ".mq4");
            }

            if (systemId == 3)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "TradingSystems", "Compile",
                                    "MaxTrade_" + user.Id + ".mq4");
            }

            return null;
        }

        private string GetTemplatePath(int systemId)
        {
            if (systemId == 1)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "TradingSystems", "Templates", "Money+eurusd.mq4");
            }

            if (systemId == 2)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "TradingSystems", "Templates", "GarantedProfit.mq4");
            }

            if (systemId == 3)
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "TradingSystems", "Templates", "MaxTrade.mq4");
            }

            return null;
        }
    }
}
