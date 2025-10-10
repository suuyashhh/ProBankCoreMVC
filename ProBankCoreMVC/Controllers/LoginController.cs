using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProBankCoreMVC.Interfaces;
using Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogin loginRepository;
        private readonly IConfiguration _config;

        // 🔐 In-memory store to track the latest valid token per user
        private static readonly ConcurrentDictionary<string, string> UserTokenStore = new();

        public LoginController(ILogin loginrepository, IConfiguration config)
        {
            loginRepository = loginrepository;
            _config = config;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] DTOLogin login)
        {
            var user = await loginRepository.Login(login);
            if (user == null) return Unauthorized("Invalid credentials");

            var jwtSettings = _config.GetSection("Jwt");
            var tokenId = Guid.NewGuid().ToString(); // Unique token ID (jti)

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, tokenId),
                new Claim(ClaimTypes.NameIdentifier, user.INI.ToString()),
                new Claim(ClaimTypes.Name, user.NAME),
                new Claim("AUTHORITY", user.AUTHORITY.ToString()),
                new Claim("ACTIVATE", user.ACTIVATE)
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            // ⚠️ Token without expiry
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            // No expiry
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 🔄 Store the new token ID and overwrite any old session
            UserTokenStore[user.INI.ToString()] = tokenId;

            return Ok(new
            {
                token = tokenString,
                userDetails = new
                {
                    user.INI,
                    user.NAME,
                    user.ACTIVATE,
                    user.AUTHORITY,
                    user.LOGIN_IP
                }
            });
        }

        [HttpPost("Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                UserTokenStore.TryRemove(userId, out _); // Invalidate the token
                return Ok("Logged out successfully");
            }

            return Unauthorized();
        }

        // 🔍 Token check middleware (copy to Program.cs or Middleware folder if needed)
        public class TokenValidationMiddleware
        {
            private readonly RequestDelegate _next;

            public TokenValidationMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var jti = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                    if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(jti))
                    {
                        if (UserTokenStore.TryGetValue(userId, out var currentJti))
                        {
                            if (currentJti != jti)
                            {
                                context.Response.StatusCode = 401;
                                await context.Response.WriteAsync("Session expired. You logged in from another device.");
                                return;
                            }
                        }
                    }
                }

                await _next(context);
            }
        }
    }
}
