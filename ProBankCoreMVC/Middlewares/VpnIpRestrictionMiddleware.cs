namespace ProBankCoreMVC.Controllers
{
    public class VpnIpRestrictionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public VpnIpRestrictionMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            // ⭐ DevMode bypass
            if (_config["Security:DevMode"] == "true")
            {
                await _next(context);
                return;
            }

            var allowedRanges = _config["Security:AllowedVpnIpRange"]?.Split(';');
            var remoteIp = context.Connection.RemoteIpAddress;

            if (allowedRanges != null)
            {
                bool allowed = allowedRanges.Any(r =>
                {
                    if (System.Net.IPAddress.TryParse(r, out var ip))
                    {
                        return ip.Equals(remoteIp);
                    }
                    return false;
                });

                if (!allowed)
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("{\"message\":\"VPN_ONLY_ACCESS\"}");
                    return;
                }
            }

            await _next(context);
        }
    }
}
