using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroccoliTrade.Domain;
using BroccoliTrade.Domain.Models;
using BroccoliTrade.Logics.Impl.Membership;
using BroccoliTrade.Logics.Interfaces.Communications;
using BroccoliTrade.Logics.MSMQ;

namespace BroccoliTrade.Logics.Impl.Communications
{
    public class CommunicationService : ICommunicationService
    {
        public BroccoliEntities db = null;

        public CommunicationService()
        {
            db = new BroccoliEntities();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public Comment GetById(int id)
        {
            return db.Comment.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Comment> GetAllConfirmedComments()
        {
            return db.Comment.Where(x => x.IsConfirmed && !x.IsDeleted);
        }

        public IEnumerable<Comment> GetAllNewComments()
        {
            return db.Comment.Where(x => !x.IsConfirmed && !x.IsDeleted);
        }

        public void AddComment(Comment entity)
        {
            db.Comment.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<UserGroups> GetAllGroups()
        {
            return db.UserGroups.Include("Mails");
        }

        public UserGroups GetGroupById(int id)
        {
            return db.UserGroups.FirstOrDefault(x => x.Id == id);
        }

        public Mails GetMailById(int id)
        {
            return db.Mails.FirstOrDefault(x => x.Id == id);
        }

        public int GetNextNumberInGroup(int groupId)
        {
            var count = db.Mails
                          .Where(x => x.GroupId == groupId && !x.IsDeleted)
                          .OrderByDescending(x => x.MailNumber)
                          .FirstOrDefault();

            return count != null ? count.MailNumber + 1 : 1;
        }

        public void SaveMail(Mails entity)
        {
            db.Mails.Add(entity);
        }

        public void RunSendMails()
        {
            var users = db.Users.Where(x => x.MailNumber <= 30).ToList();

            foreach (var user in users)
            {
                var mail = this.GetMailByGroupAndNumber(user.GroupId, user.MailNumber);

                var em = new EmailMessage
                {
                    Subject = mail.MailName,
                    Message = mail.MailBody.Replace("%ИМЯ%", user.Name),
                    From = "support@broccoli-trade.ru",
                    DisplayNameFrom = "Broccoli Trade",
                    To = user.Email
                };

                new QueueService().QueueMessage(em);

                user.MailNumber = user.MailNumber + 1;
                this.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        private Mails GetMailByGroupAndNumber(int groupId, int number)
        {
            return db.Mails.FirstOrDefault(x => x.GroupId == groupId && x.MailNumber == number);
        }
    }
}
