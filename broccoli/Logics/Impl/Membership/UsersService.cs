﻿using System;
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
            entity.RegisterDate = DateTime.Now;
            entity.IsDeleted = false;

            db.Users.Add(entity);
            db.SaveChanges();
        }

        public Users GetUserByLogin(string login)
        {
            return db.Users.FirstOrDefault(x => x.Email == login);
        }

        public bool EmailIsExist(string email)
        {
            return db.Users.FirstOrDefault(x => x.Email == email && !x.IsDeleted) != null;
        }

        public bool NicknameIsExist(string nickname)
        {
            return db.Users.FirstOrDefault(x => x.Nickname == nickname && !x.IsDeleted) != null;
        }

        public bool PhoneIsExist(string phone)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
