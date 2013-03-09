using System;
using BroccoliTrade.Domain;

namespace BroccoliTrade.Logics.Interfaces.Membership
{
    /// <summary>
    /// Interface for managing user's database
    /// </summary>
    public interface IUsersService : IDisposable
    {
        /// <summary>
        /// Inserts new entity into database
        /// </summary>
        /// <param name="entity"></param>
        void Insert(Users entity);

        /// <summary>
        /// Возвращает пользователя по id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Users GetById(long userId);

        /// <summary>
        /// Get entity user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Users GetUserByLogin(string email);

        /// <summary>
        /// Get entity user by email hash
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        Users GetUserByEmailHash(string hash);

        /// <summary>
        /// Check email in db
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool EmailIsExist(string email);

        /// <summary>
        /// Check nickname in db
        /// </summary>
        /// <param name="nickname"></param>
        /// <returns></returns>
        bool NicknameIsExist(string nickname);

        /// <summary>
        /// Check pnone number in db
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        bool PhoneIsExist(string phone);

        /// <summary>
        /// Change password
        /// </summary>
        void Update(Users user);

        /// <summary>
        /// Save
        /// </summary>
        void SaveChanges();
    }
}
