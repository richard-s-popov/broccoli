﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using BroccoliTrade.Domain;
using BroccoliTrade.Domain.Models;
using BroccoliTrade.Logics.Core;
using BroccoliTrade.Logics.Infrastructure.Certificates;
using BroccoliTrade.Logics.Interfaces.Account;
using BroccoliTrade.Logics.MSMQ;
using Newtonsoft.Json.Linq;

namespace BroccoliTrade.Logics.Impl.Account
{
    public class AccountsService : IAccountsService
    {
        public BroccoliEntities db = null;

        public AccountsService()
        {
            db = new BroccoliEntities();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public Accounts GetById(int id)
        {
            return db.Accounts
                .Include("Users")
                .FirstOrDefault(x => x.Id == id);
        }

        public Accounts GetByAccountNumber(string account)
        {
            return db.Accounts
                .Include("Users")
                .FirstOrDefault(x => x.AccountNumber == account && !x.IsDeleted);
        }

        public IEnumerable<Accounts> GetAccountsByUserId(long id)
        {
            return db.Accounts
                .Include("Users")
                .Where(x => x.UserId == id && !x.IsDeleted);
        }

        public void CreateAccount(Accounts entity)
        {
            db.Accounts.Add(entity);
            db.SaveChanges();
        }

        public bool AccountIsActivated(int id)
        {
            return db.Accounts.FirstOrDefault(x => x.Status.Id == 2 && !x.IsDeleted) != null;
        }

        public bool AccountIsBusy(int id)
        {
            return db.Accounts.FirstOrDefault(x => x.Status.Id == 1 && !x.IsDeleted) != null;
        }

        public void UpdateAccount(Accounts entity)
        {
            var acc = db.Accounts.First(x => x.Id == entity.Id);

            acc.IsNew = entity.IsNew;
            acc.ModifiedDate = DateTime.Now;
            acc.StatusId = entity.StatusId;
            acc.UserId = entity.UserId;
            acc.CreateDate = entity.CreateDate;
            acc.AccountNumber = entity.AccountNumber;
            acc.IsDeleted = entity.IsDeleted;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void AddAccountToApplicationsPool(AccountPool entity)
        {
            db.AccountPool.Add(entity);
            db.SaveChanges();
        }

        public void AccountCountWorker()
        {
            try
            {
                var accounts = db.Accounts.Where(x => x.StatusId == 2 && x.Broker == 1).ToList();

                ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

                foreach (var userAccount in accounts)
                {
                    try
                    {
                        var myHttpWebRequest = (HttpWebRequest) WebRequest
                                                                    .Create(
                                                                        string.Format(
                                                                            "https://my.forexinn.ru/agent-api/check-involved-account/broccoli/jcmbogje9b5uxs/{0}",
                                                                            userAccount.AccountNumber));

                        var myHttpWebResponse = (HttpWebResponse) myHttpWebRequest.GetResponse();

                        var myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.UTF8);

                        var response = myStreamReader.ReadToEnd();
                        dynamic json = JToken.Parse(response);

                        if ((bool) json["result"] && !(bool) json["demo"])
                        {
                            var account = this.GetById(userAccount.Id);
                            account.StatusId = 2;
                            account.IsNew = true;

                            // Если пользователь пополнил при этом счет больше 1000, то переносим ему в группу
                            if ((double) json["count"] > 1000 && userAccount.Users.GroupId < 4)
                            {
                                account.Users.GroupId = 4;
                            }
                            else if ((double) json["count"] < 1000 && (double) json["count"] > 0 &&
                                     userAccount.Users.GroupId < 3)
                            {
                                account.Users.GroupId = 3;
                            }
                            else if (userAccount.Users.GroupId == 1)
                            {
                                account.Users.GroupId = 2;
                            }
                        }

                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CheckAccountsInPool()
        {
            try
            {
                // var time = DateTime.Now.AddMinutes(-1 * GetRandomMinutes(5));
                var accounts = db.AccountPool.Select(x => x).ToList();

                ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

                foreach (var AccountPool in accounts)
                {
                    if (AccountPool.Accounts.Broker == 1)
                    {
                        var myHttpWebRequest = (HttpWebRequest) WebRequest
                                                                    .Create(
                                                                        string.Format(
                                                                            "https://my.forexinn.ru/agent-api/check-involved-account/broccoli/jcmbogje9b5uxs/{0}",
                                                                            AccountPool.AccountNumber));

                        var myHttpWebResponse = (HttpWebResponse) myHttpWebRequest.GetResponse();

                        var myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream(), Encoding.UTF8);

                        var response = myStreamReader.ReadToEnd();
                        dynamic json = JToken.Parse(response);

                        if ((bool) json["result"])
                        {
                            var account = this.GetById(AccountPool.AccountId);
                            account.StatusId = 2;
                            account.IsNew = true;

                            if (!(bool) json["demo"])
                            {
                                // Если пользователь пополнил при этом счет больше 1000, то переносим ему в группу
                                if ((double)json["count"] > 1000)
                                {
                                    account.Users.GroupId = 4;
                                }
                                else if ((double)json["count"] < 1000 && (double)json["count"] > 0)
                                {
                                    account.Users.GroupId = 3;
                                }
                                else
                                {
                                    account.Users.GroupId = 2;
                                }
                            }
                            
                            var em = new EmailMessage
                                {
                                    Subject = string.Format("Счет {0} активирован", account.AccountNumber),
                                    Message =
                                        string.Format(
                                            "Здравствуйте, {0}. Счет {1} брокера Forexinn успешно активирован.",
                                            account.Users.Name, account.AccountNumber),
                                    From = "support@broccoli-trade.ru",
                                    DisplayNameFrom = "Broccoli Trade",
                                    To = account.Users.Email
                                };

                            new QueueService().QueueMessage(em);
                        }
                        else
                        {
                            var account = this.GetById(AccountPool.AccountId);
                            account.StatusId = 3;
                            account.IsNew = true;

                            switch ((int) json["reason"])
                            {
                                case 1:
                                    account.Reason = "Счет не существует. Проверьте правильность указанного счета.";
                                    break;
                                case 2:
                                    account.Reason =
                                        "Счет создан без нашего партнерского кода \"broccoli\". Создайте счет у брокера с нашим партнерским кодом, либо воспользуйтесь кнопкой \"открыть счет\" у нас на сайте";
                                    break;
                                case 8:
                                    account.Reason = "Неверно указан наш партнерский код.";
                                    break;
                                default:
                                    account.Reason = null;
                                    break;
                            }

                            var em = new EmailMessage
                                {
                                    Subject = string.Format("Счет {0} отклонен", account.AccountNumber),
                                    Message =
                                        string.Format("Здравствуйте, {0}. Счет {1} отклонен.<br/><b>{2}</b>",
                                                      account.Users.Name, account.AccountNumber, account.Reason),
                                    From = "support@broccoli-trade.ru",
                                    DisplayNameFrom = "Broccoli Trade",
                                    To = account.Users.Email
                                };

                            new QueueService().QueueMessage(em);
                        }

                        db.AccountPool.Remove(AccountPool);
                        db.SaveChanges();
                        continue;
                    }

                    if (AccountPool.Accounts.Broker == 2)
                    {
                        var instaforexAccounts = InstaforexAPI.GetAccounts();
                            
                        if (instaforexAccounts.Any(x => x.ToString() == AccountPool.AccountNumber))
                        {
                            var account = this.GetById(AccountPool.AccountId);
                            account.StatusId = 2;
                            account.IsNew = true;

                            var em = new EmailMessage
                            {
                                Subject = string.Format("Счет {0} активирован", account.AccountNumber),
                                Message = string.Format("Здравствуйте, {0}. Счет {1} брокера Instaforex успешно активирован.", account.Users.Name, account.AccountNumber),
                                From = "support@broccoli-trade.ru",
                                DisplayNameFrom = "Broccoli Trade",
                                To = account.Users.Email
                            };

                            new QueueService().QueueMessage(em);
                        }
                        else
                        {
                            var account = this.GetById(AccountPool.AccountId);
                            account.StatusId = 3;
                            account.IsNew = true;

                            account.Reason = "Счет не зарегистрирован у брокера или зарегистрирован не по нашему партнерскому коду \"broccoli\"";

                            var em = new EmailMessage
                            {
                                Subject = string.Format("Счет {0} отклонен", account.AccountNumber),
                                Message = string.Format("Здравствуйте, {0}. Счет {1} отклонен.<br/><b>{2}</b>", account.Users.Name, account.AccountNumber, account.Reason),
                                From = "support@broccoli-trade.ru",
                                DisplayNameFrom = "Broccoli Trade",
                                To = account.Users.Email
                            };

                            new QueueService().QueueMessage(em);
                        }

                        db.AccountPool.Remove(AccountPool);
                        db.SaveChanges();
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        public void DeleteAccount(Accounts account)
        {
            var acc = db.Accounts.First(x => x.Id == account.Id);
            acc.IsDeleted = true;
            db.SaveChanges();
        }

        public int GetNewNotoficationsCountByUserId(long userId)
        {
            return db.Accounts.Count(x => x.IsNew && x.UserId == userId && !x.IsDeleted);
        }

        private int GetRandomMinutes(int range)
        {
            var random = new Random();

            return random.Next(range);
        }

        private void Log(string msg)
        {
            File.AppendAllText(@"C:\broccoli_log.txt", DateTime.Now + " " + msg + Environment.NewLine);
        }
    }
}
