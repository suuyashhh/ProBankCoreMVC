using System.Security.Cryptography;
using System.Text;

namespace ProBankCoreMVC.Controllers
{
    public class HmacValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secret;
        private readonly int _windowSeconds;

        public HmacValidationMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _secret = config["Security:HmacSecret"] ?? "";
            _windowSeconds = int.Parse(config["Security:RequestTimeWindowSeconds"] ?? "60");
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";

            // Skip OPTIONS
            if (HttpMethods.IsOptions(context.Request.Method))
            {
                await _next(context);
                return;
            }

            // Skip LOGIN (no HMAC required)
            if (path.EndsWith("/api/login/login", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            // Must include HMAC headers
            if (!context.Request.Headers.TryGetValue("X-REQ-TIME", out var ts) ||
                !context.Request.Headers.TryGetValue("X-REQ-SIGN", out var sign))
            {
                await Reject(context, "MISSING_HMAC_HEADERS");
                return;
            }

            if (!long.TryParse(ts, out var sentUnix))
            {
                await Reject(context, "INVALID_TIMESTAMP");
                return;
            }

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (Math.Abs(now - sentUnix) > _windowSeconds)
            {
                await Reject(context, "REQUEST_EXPIRED");
                return;
            }

            // Enable body reading
            context.Request.EnableBuffering();
            string body = "";
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            string method = context.Request.Method.ToUpperInvariant();
            string route = path.ToLowerInvariant();

            string payload = $"{method}\n{route}\n{sentUnix}\n{body}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var expected = Convert.ToHexString(hash);

            if (!CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(expected),
                Encoding.UTF8.GetBytes(sign)))
            {
                await Reject(context, "INVALID_HMAC_SIGNATURE");
                return;
            }

            await _next(context);
        }

        private static async Task Reject(HttpContext context, string msg)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync($"{{\"message\":\"{msg}\"}}");
        }
    }
}
