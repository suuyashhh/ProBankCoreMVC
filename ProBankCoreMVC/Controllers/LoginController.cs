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

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _loginRepository;
        private readonly IConfiguration _config;

        public LoginController(ILogin loginRepository, IConfiguration config)
        {
            _loginRepository = loginRepository;
            _config = config;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] DTOLogin login)
        {
            if (login == null || string.IsNullOrEmpty(login.INI) || string.IsNullOrEmpty(login.CODE))
                return BadRequest(new { message = "INI and CODE are required." });

            // 1) Validate credentials via the repository
            DTOLogin? user;
            try
            {
                user = await _loginRepository.LoginAsync(login);
            }
            catch (Exception ex)
            {
                // If DB error, return 500
                return StatusCode(500, new { message = "Server error while validating credentials.", detail = ex.Message });
            }

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // 2) Read JWT settings
            var jwtSection = _config.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expiryMinutes = jwtSection.GetValue<int>("ExpiryMinutes", 20); // default to 20 if not set

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                return StatusCode(500, new { message = "JWT not configured properly on server." });
            }

            // 3) Create claims and a unique JTI
            var jti = Guid.NewGuid().ToString();

            // Use INI as NameIdentifier (since your DTO does not contain numeric USER_ID)
            var nameIdentifier = user.INI ?? login.INI ?? string.Empty;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(ClaimTypes.NameIdentifier, nameIdentifier),
                new Claim(ClaimTypes.Name, user.NAME ?? string.Empty),
                new Claim("AUTHORITY", user.AUTHORITY ?? string.Empty),
                new Claim("ACTIVATE", user.ACTIVATE ?? string.Empty)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // 4) Persist the JTI for single-device enforcement (use INI or nameIdentifier)
            try
            {
                if (!string.IsNullOrEmpty(nameIdentifier))
                {
                    await _loginRepository.SetUserJtiAsync(nameIdentifier, jti);
                }
            }
            catch (Exception ex)
            {
                // If JTI persist fails, still return token but log/return error depends on your policy
                // For safety, return 500 so clients don't get token that won't validate later
                return StatusCode(500, new { message = "Server error while storing session data.", detail = ex.Message });
            }

            // 5) Return token and user details to the client
            return Ok(new
            {
                token = tokenString,
                expires = expires,
                userDetails = new
                {
                    user.INI,
                    user.LOGIN_IP,
                    user.NAME,
                    user.AUTHORITY,
                    user.ACTIVATE
                }
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // If authentication is enabled, the NameIdentifier claim will be set
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Not authenticated" });

            try
            {
                // Clear JTI to invalidate current token(s)
                await _loginRepository.SetUserJtiAsync(userId, null);
                return Ok(new { message = "Logged out" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error during logout", detail = ex.Message });
            }
        }
    }
}
