using System;
using System.Web;

namespace BroccoliTrade.Logics.Interfaces.Membership
{
    /// <summary>
    /// Service for handling user's logging in and out
    /// </summary>
    public interface IMembershipService : IDisposable
    {
        /// <summary>
        /// Authorizes user
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>True if such user exists in database and not blocked, false otherwise</returns>
        bool AuthorizeUser(string login, string password);

        /// <summary>
        /// Logs user in 
        /// </summary>
        /// <param name="currentHttpContext">Current HTTP context to store user's information and cookies</param>
        /// <param name="login">User's login</param>
        /// <param name="password">User's password</param>
        void LoginUser(HttpContext currentHttpContext, string login, string password, bool createPersistentCookie);

        /// <summary>
        /// Logs current user out
        /// </summary>
        /// <param name="currentHttpContext"></param>
        void LogoutCurrentUser(HttpContext currentHttpContext);
    }
}
