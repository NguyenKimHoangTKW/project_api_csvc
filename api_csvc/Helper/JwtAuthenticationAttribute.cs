using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace api_csvc.Helper
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
            {
                // No authentication header found
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                // Token is missing
                context.ErrorResult = new AuthenticationFailureResult("Missing token", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = JwtService.ValidateToken(token);

            if (principal == null)
            {
                // Invalid token
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
                return;
            }

            // Set the principal
            context.Principal = principal;
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer");
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
        }
    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public string ReasonPhrase { get; }
        public HttpRequestMessage Request { get; }

        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = Request,
                ReasonPhrase = ReasonPhrase,
                Content = new StringContent($"{{\"message\": \"{ReasonPhrase}\", \"success\": false}}", 
                    System.Text.Encoding.UTF8, "application/json")
            };
            return response;
        }
    }

    public class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {
        private readonly System.Net.Http.Headers.AuthenticationHeaderValue _challenge;
        private readonly IHttpActionResult _innerResult;

        public AddChallengeOnUnauthorizedResult(System.Net.Http.Headers.AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            _challenge = challenge;
            _innerResult = innerResult;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await _innerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Headers.WwwAuthenticate.Add(_challenge);
            }

            return response;
        }
    }
} 