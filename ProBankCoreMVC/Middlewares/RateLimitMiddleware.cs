using System.Collections.Concurrent;

namespace ProBankCoreMVC.Controllers
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, (DateTime ts, int count)> _rate = new();

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "UNKNOWN";
            var now = DateTime.UtcNow;

            var entry = _rate.GetOrAdd(ip, _ => (now, 1));

            if ((now - entry.ts).TotalSeconds <= 10)
            {
                if (entry.count > 30) // 30 requests / 10 sec
                {
                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsync("{\"message\":\"RATE_LIMIT_EXCEEDED\"}");
                    return;
                }
                _rate[ip] = (entry.ts, entry.count + 1);
            }
            else
            {
                _rate[ip] = (now, 1);
            }

            await _next(context);
        }
    }
}
