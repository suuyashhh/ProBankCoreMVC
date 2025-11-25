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
        private readonly ConcurrentDictionary<string, string> _userTokenStore;

        public LoginController(ILogin loginrepository, IConfiguration config, ConcurrentDictionary<string, string> userTokenStore)
        {
            loginRepository = loginrepository;
            _config = config;
            _userTokenStore = userTokenStore;
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
                new Claim("WORKING_BRANCH", user.WORKING_BRANCH.ToString()),
                new Claim("ALLOW_BR_SELECTION", user.ALLOW_BR_SELECTION)
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            // Token (no expiry for simplicity) 
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // ✅ Overwrite any existing session for that user 
            _userTokenStore[user.INI.ToString()] = tokenId;

            return Ok(new
            {
                token = tokenString,
                userDetails = new
                {
                    user.INI,
                    user.NAME,
                    user.ALLOW_BR_SELECTION,
                    user.WORKING_BRANCH,
                    user.LOGIN_IP,
                    user.DESIGNATION,
                    user.USER_LAVEL
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
                _userTokenStore.TryRemove(userId, out _);
                return Ok("Logged out successfully");
            }
            return Unauthorized();
        }

        [HttpGet("CheckAuthorize")]
        [Authorize]
        public IActionResult CheckAuthorize()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jti = User.FindFirstValue(JwtRegisteredClaimNames.Jti);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(jti))
                return Unauthorized(new { message = "INVALID_TOKEN" });

            if (_userTokenStore.TryGetValue(userId, out var currentJti))
            {
                if (currentJti != jti)
                    return Unauthorized(new { message = "LOGGED_OUT_OTHER_DEVICE" });
            }

            return Ok(new { authorized = true });
        }


    }

    // 🔍 Middleware to validate tokens dynamically 
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
                            context.Response.StatusCode = 401;
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