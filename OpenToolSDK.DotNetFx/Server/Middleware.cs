using Microsoft.Owin;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OpenToolSDK.DotNetFx.Server
{
    public class AuthorizationMiddleware : OwinMiddleware
    {
        private readonly HashSet<string> _validApiKeys;
        private const string TokenPrefix = "Bearer ";

        public AuthorizationMiddleware(OwinMiddleware next, IEnumerable<string> validApiKeys)
            : base(next)
        {
            _validApiKeys = new HashSet<string>(validApiKeys);
        }

        public override async Task Invoke(IOwinContext context)
        {
            var authHeader = context.Request.Headers.Get("Authorization");

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith(TokenPrefix))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Missing or malformed authorization header");
                return;
            }

            var token = authHeader.Substring(TokenPrefix.Length);
            if (!_validApiKeys.Contains(token))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid authorization token");
                return;
            }

            await Next.Invoke(context);
        }
    }
}
