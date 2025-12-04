using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;

namespace ProBankCoreMVC.Controllers
{
    public class SecurityMonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SecurityMonitoringMiddleware> _logger;
        private static readonly ConcurrentDictionary<string, int> _ipFailureCount = new();

        public SecurityMonitoringMiddleware(RequestDelegate next, ILogger<SecurityMonitoringMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "UNKNOWN";
            var path = context.Request.Path.Value ?? "UNKNOWN";

            await _next(context);

            // Track suspicious access
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized ||
                context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                var count = _ipFailureCount.AddOrUpdate(ip, 1, (_, c) => c + 1);

                _logger.LogWarning(
                    "⚠️ POSSIBLE INTRUSION | IP: {ip} | Attempts: {count} | Path: {path}",
                    ip, count, path
                );

                // Optional alert threshold
                if (count > 10)
                {
                    _logger.LogError(
                        "🚨 CRITICAL ALERT: Repeated unauthorized access from {ip}",
                        ip
                    );
                }
            }
        }
    }
}
