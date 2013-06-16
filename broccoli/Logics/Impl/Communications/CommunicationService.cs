using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroccoliTrade.Domain;
using BroccoliTrade.Logics.Interfaces.Communications;

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
            return db.Mails.Where(x => x.GroupId == groupId && !x.IsDeleted).Max(x => x.MailNumber) + 1;
        }

        public void SaveMail(Mails entity)
        {
            db.Mails.Add(entity);
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
