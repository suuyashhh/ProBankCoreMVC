using System.Text.Json;

namespace ProBankCoreMVC.Controllers
{
    public class AppKeyValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _expectedAppKey;

        public AppKeyValidationMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _expectedAppKey = config["Security:AppKey"] ?? "";
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";

            // Allow CORS OPTIONS
            if (HttpMethods.IsOptions(context.Request.Method))
            {
                await _next(context);
                return;
            }

            // Allow LOGIN without app key
            if (path.EndsWith("/api/login/login", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            // Validate app key
            if (!context.Request.Headers.TryGetValue("X-APP-KEY", out var provided) ||
                provided != _expectedAppKey)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"message\":\"INVALID_APP_CLIENT\"}");
                return;
            }

            await _next(context);
        }
    }
}
