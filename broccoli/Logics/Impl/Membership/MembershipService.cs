using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using BroccoliTrade.Logics.Interfaces.Membership;
using BroccoliTrade.Logics.Infrastructure.Extensions;
using BroccoliTrade.Domain;

namespace BroccoliTrade.Logics.Impl.Membership
{
    public class MembershipService : IMembershipService
    {
        public BroccoliEntities db = null;

        public MembershipService()
        {
            db = new BroccoliEntities();
        }

        public bool AuthorizeUser(string login, string password)
        {
            var pass = password.Md5();
            return db.Users.FirstOrDefault(x => x.Email == login && x.Password == pass) != null;
        }

        public void LoginUser(HttpContext currentHttpContext, string login, string password, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "login");

            FormsAuthentication.SetAuthCookie(login, createPersistentCookie);
        }

        public void LogoutCurrentUser(HttpContext currentHttpContext)
        {
            FormsAuthentication.SignOut();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
