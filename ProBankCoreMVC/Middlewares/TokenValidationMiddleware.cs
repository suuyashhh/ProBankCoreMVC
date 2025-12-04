using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Concurrent;

namespace ProBankCoreMVC.Controllers
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ConcurrentDictionary<string, string> _userTokenStore;

        public TokenValidationMiddleware(RequestDelegate next, ConcurrentDictionary<string, string> userTokenStore)
        {
            _next = next;
            _userTokenStore = userTokenStore;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var jti = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(jti))
                {
                    if (_userTokenStore.TryGetValue(userId, out var currentJti))
                    {
                        if (currentJti != jti)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync("{\"message\":\"LOGGED_OUT_OTHER_DEVICE\"}");
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
