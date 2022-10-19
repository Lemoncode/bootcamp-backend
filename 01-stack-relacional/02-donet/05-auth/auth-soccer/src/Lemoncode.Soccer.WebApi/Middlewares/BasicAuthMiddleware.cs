using System.Threading.Tasks;
using Lemoncode.Soccer.Application.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Lemoncode.Soccer.WebApi.Middlewares
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IConfiguration configuration)
        {
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authHeader))
            {
                httpContext.Response.StatusCode = 401;
                return;
            }

            var isValidBasicAuth = AuthorizationUtil.TryGetCredentials(authHeader, out var username, out var password);
            if (!isValidBasicAuth)
            {
                httpContext.Response.StatusCode = 401;
                return;
            }

            var storedUsername = configuration.GetValue<string>("BasicAuthentication:Username");
            var storedPassword = configuration.GetValue<string>("BasicAuthentication:Password");

            var isValidCredentials = username == storedUsername && password == storedPassword;
            if (!isValidCredentials)
            {
                httpContext.Response.StatusCode = 401;
                return;
            }

            // otherwise all is good to continue processing the http request
            await _next(httpContext);
        }
    }
}
