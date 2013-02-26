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

        public IEnumerable<Comment> GetAllConfirmedComments()
        {
            return db.Comment.Where(x => x.IsConfirmed);
        }

        public void AddComment(Comment entity)
        {
            db.Comment.Add(entity);
            db.SaveChanges();
        }
    }
}
