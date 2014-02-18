﻿using System;
using System.Collections.Generic;
using System.Linq;
using BroccoliTrade.Logics.Infrastructure.Extensions;
using BroccoliTrade.Logics.Interfaces.Membership;
using BroccoliTrade.Domain;

namespace BroccoliTrade.Logics.Impl.Membership
{
    public class UsersService : IUsersService
    {
        public BroccoliEntities db = null;

        public UsersService()
        {
            db = new BroccoliEntities();
        }

        public void Insert(Users entity)
        {
            entity.Password = entity.Password.Md5();
            entity.EmailHash = entity.Email.Md5();
            entity.RegisterDate = DateTime.Now;
            entity.IsDeleted = false;

            db.Users.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<Users> GetAllUsers()
        {
            return db.Users.Where(x => !x.IsDeleted);
        }

        public Users GetById(long userId)
        {
            return db.Users.FirstOrDefault(x => x.Id == userId);
        }

        public Users GetUserByLogin(string login)
        {
            return db.Users.FirstOrDefault(x => x.Email == login && !x.IsDeleted);
        }

        public Users GetUserByEmailHash(string hash)
        {
            return db.Users.FirstOrDefault(x => x.EmailHash == hash && !x.IsDeleted);
        }

        public bool EmailIsExist(string email)
        {
            return db.Users.FirstOrDefault(x => x.Email == email && !x.IsDeleted) != null;
        }

        public bool PhoneIsExist(string phone)
        {
            throw new NotImplementedException();
        }

        public void Update(Users user)
        {
            var entity = db.Users.First(x => x.Id == user.Id);

            entity.Password = user.Password;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
