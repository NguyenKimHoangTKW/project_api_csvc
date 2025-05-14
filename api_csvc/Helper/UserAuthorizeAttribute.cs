using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace api_csvc.Helper
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly int[] _allowedUserTypes;

        public UserAuthorizeAttribute(params int[] allowedUserTypes)
        {
            _allowedUserTypes = allowedUserTypes ?? new int[0];
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = SessionHelper.GetUser();
            if (user == null || user.id_role == null)
            {
                return false;
            }
            if (_allowedUserTypes.Length > 0 && !_allowedUserTypes.Contains(user.id_role.Value))
            {
                return false;
            }
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/trang-chu");
        }
    }
}