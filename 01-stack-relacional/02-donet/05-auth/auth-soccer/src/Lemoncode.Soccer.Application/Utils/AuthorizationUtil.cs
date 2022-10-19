using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Lemoncode.Soccer.Application.Utils
{
    public static class AuthorizationUtil
    {
        public static bool TryGetCredentials(string authorizationHeader, out string username, out string password)
        {
            var authHeaderValue = AuthenticationHeaderValue.Parse(authorizationHeader);
            if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                var credentials =
                    Encoding.UTF8
                        .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                        .Split(new[] { ':' }, 2);
                if (credentials.Length == 2)
                {
                    username = credentials[0];
                    password = credentials[1];
                    return true;
                }
            }

            username = string.Empty;
            password = string.Empty;
            return false;
        }
    }
}
