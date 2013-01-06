using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BroccoliTrade.Domain;
using BroccoliTrade.Logics.Interfaces.Statistics;

namespace BroccoliTrade.Logics.Impl.Statistics
{
    public class StatService : IStatService
    {
        public BroccoliEntities db = null;

        public StatService()
        {
            db = new BroccoliEntities();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<Referrer> GetReferrersByUserId(long userId)
        {
            return db.Referrer.Where(x => x.OwnerId == userId && !x.IsDeleted);
        }

        public Referrer GetReferrerByUserAndHost(long userId, string host)
        {
            return db.Referrer.FirstOrDefault(x => x.OwnerId == userId && x.Host == host);
        }

        public Referrer GetById(int id)
        {
            return db.Referrer.FirstOrDefault(x => x.Id == id);
        }

        public void AddReferrer(Referrer entity)
        {
            db.Referrer.Add(entity);
            db.SaveChanges();
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
