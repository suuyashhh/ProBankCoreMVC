using System.Net;

namespace ProBankCoreMVC.Controllers
{
    public class IpAllowListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<IPNetwork> _allowed = new();

        public IpAllowListMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;

            var ranges = config.GetSection("Security:AllowedApiIpRanges")
                               .Get<string[]>() ?? Array.Empty<string>();

            foreach (var r in ranges)
                if (IPNetwork.TryParse(r, out var net))
                    _allowed.Add(net);
        }

        public async Task Invoke(HttpContext context)
        {
            // Allow Swagger in dev (optional, keep if you like)
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            // If nothing configured, allow all
            if (_allowed.Count == 0)
            {
                await _next(context);
                return;
            }

            var ip = GetClientIp(context);

            // ✅ Always allow local machine (IPv4 and IPv6 loopback)
            if (ip != null && IPAddress.IsLoopback(ip))
            {
                await _next(context);
                return;
            }

            // (Optional) DEBUG – see what IP you are actually getting
            // Console.WriteLine($"[IpAllowList] Remote IP: {ip}");

            if (ip == null || !_allowed.Any(a => a.Contains(ip)))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"message\":\"IP_NOT_ALLOWED\"}");
                return;
            }

            await _next(context);
        }

        private IPAddress? GetClientIp(HttpContext ctx)
        {
            // If running behind proxy / IIS ARR
            if (ctx.Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
            {
                var ip = forwarded.ToString().Split(',').First().Trim();
                if (IPAddress.TryParse(ip, out var realIp))
                    return realIp;
            }

            return ctx.Connection.RemoteIpAddress;
        }

        // Simple IPv4 CIDR implementation
        private struct IPNetwork
        {
            public IPAddress Net;
            public IPAddress Mask;

            public static bool TryParse(string cidr, out IPNetwork net)
            {
                net = default;

                var parts = cidr.Split('/');
                if (parts.Length != 2)
                    return false;

                if (!IPAddress.TryParse(parts[0], out var baseIp))
                    return false;

                if (!int.TryParse(parts[1], out var prefix))
                    return false;

                // We only support IPv4 here
                if (baseIp.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                    return false;

                var mask = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    int bits = Math.Min(8, prefix - i * 8);
                    bits = Math.Clamp(bits, 0, 8);
                    mask[i] = (byte)~(0xFF >> bits);
                }

                var maskIp = new IPAddress(mask);
                var netAddr = new IPAddress(
                    baseIp.GetAddressBytes()
                          .Zip(mask, (b, m) => (byte)(b & m))
                          .ToArray());

                net = new IPNetwork { Net = netAddr, Mask = maskIp };
                return true;
            }

            public bool Contains(IPAddress ip)
            {
                if (ip.AddressFamily != Net.AddressFamily)
                    return false;

                var ipBytes = ip.GetAddressBytes();
                var netBytes = Net.GetAddressBytes();
                var maskBytes = Mask.GetAddressBytes();

                for (int i = 0; i < maskBytes.Length; i++)
                {
                    if ((ipBytes[i] & maskBytes[i]) != netBytes[i])
                        return false;
                }

                return true;
            }
        }
    }
}
