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

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
