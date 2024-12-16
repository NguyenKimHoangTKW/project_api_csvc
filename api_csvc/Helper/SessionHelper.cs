using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_csvc.Helper
{
    public static class SessionHelper
    {
        private const string UserInfoSessionKey = "UserInfoSessionKey";
        private const string UserRoleSessionKey = "UserRoleSessionKey";

        public static void SetUser(api_csvc.Models.Account user)
        {
            HttpContext.Current.Session[UserInfoSessionKey] = user;
            HttpContext.Current.Session[UserRoleSessionKey] = user.id_role;
        }

        public static api_csvc.Models.Account GetUser()
        {
            return HttpContext.Current.Session[UserInfoSessionKey] as api_csvc.Models.Account;
        }

        public static string GetUserRole()
        {
            return HttpContext.Current.Session[UserRoleSessionKey] as string;
        }

        public static void ClearUser()
        {
            HttpContext.Current.Session.Remove(UserInfoSessionKey);
            HttpContext.Current.Session.Remove(UserRoleSessionKey);
        }

        public static bool IsUserLoggedIn()
        {
            return HttpContext.Current.Session[UserInfoSessionKey] != null;
        }
    }
}