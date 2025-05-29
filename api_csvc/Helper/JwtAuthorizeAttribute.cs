using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace api_csvc.Helper
{
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int[] _allowedRoles;

        public JwtAuthorizeAttribute(params int[] allowedRoles)
        {
            _allowedRoles = allowedRoles ?? new int[0];
        }

        public bool AllowMultiple => false;

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext, 
            CancellationToken cancellationToken, 
            Func<Task<HttpResponseMessage>> continuation)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            
            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return CreateUnauthorizedResponse(actionContext.Request, "Authentication required");
            }

            // If no specific roles are required, just check if user is authenticated
            if (_allowedRoles.Length == 0)
            {
                return await continuation();
            }

            // Check if user has required role
            var roleClaim = principal.FindFirst("id_role");
            if (roleClaim == null)
            {
                return CreateUnauthorizedResponse(actionContext.Request, "Role information not found");
            }

            if (!int.TryParse(roleClaim.Value, out int userRole))
            {
                return CreateUnauthorizedResponse(actionContext.Request, "Invalid role information");
            }

            if (!_allowedRoles.Contains(userRole))
            {
                return CreateForbiddenResponse(actionContext.Request, "Insufficient permissions");
            }

            return await continuation();
        }

        private HttpResponseMessage CreateUnauthorizedResponse(HttpRequestMessage request, string message)
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = request,
                Content = new StringContent($"{{\"message\": \"{message}\", \"success\": false}}", 
                    System.Text.Encoding.UTF8, "application/json")
            };
        }

        private HttpResponseMessage CreateForbiddenResponse(HttpRequestMessage request, string message)
        {
            return new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                RequestMessage = request,
                Content = new StringContent($"{{\"message\": \"{message}\", \"success\": false}}", 
                    System.Text.Encoding.UTF8, "application/json")
            };
        }
    }

    // Extension methods to help get user information from the current principal
    public static class JwtPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }

        public static int? GetUserRole(this ClaimsPrincipal principal)
        {
            var roleClaim = principal?.FindFirst("id_role");
            if (roleClaim != null && int.TryParse(roleClaim.Value, out int roleId))
            {
                return roleId;
            }
            return null;
        }

        public static string GetUsername(this ClaimsPrincipal principal)
        {
            return principal?.FindFirst("username")?.Value;
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static string GetName(this ClaimsPrincipal principal)
        {
            return principal?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
} 