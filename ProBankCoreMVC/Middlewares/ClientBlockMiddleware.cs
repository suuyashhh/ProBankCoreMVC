namespace ProBankCoreMVC.Controllers
{
    public class ClientBlockMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public ClientBlockMiddleware(RequestDelegate next, IConfiguration config)
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

            var ua = context.Request.Headers["User-Agent"]
                            .ToString()
                            .ToLower();

            if (ua.Contains("postman") ||
                ua.Contains("curl") ||
                ua.Contains("python") ||
                ua.Contains("insomnia"))
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("{\"message\":\"BLOCKED_NON_APP_CLIENT\"}");
                return;
            }

            await _next(context);
        }
    }
}
