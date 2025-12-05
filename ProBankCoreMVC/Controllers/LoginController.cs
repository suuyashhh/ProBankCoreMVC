using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using ProBankCoreMVC.Helpers;
using ProBankCoreMVC.Interfaces;

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

        public class LoginEncryptedDTO
        {
            public string data { get; set; }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginEncryptedDTO model)
        {
            try
            {
                // 0️⃣ Validate encrypted payload exists
                if (model == null || string.IsNullOrWhiteSpace(model.data))
                    return BadRequest("INVALID_ENCRYPTED_PAYLOAD");

                // 1️⃣ Read headers
                string appKey = Request.Headers["X-APP-KEY"];
                string deviceId = Request.Headers["X-DEVICE-ID"];
                string timestamp = Request.Headers["X-TIMESTAMP"];
                string signature = Request.Headers["X-SIGNATURE"];

                // 2️⃣ Validate AppKey
                string expectedAppKey = _config["Security:AppKey"];
                if (appKey != expectedAppKey)
                    return Unauthorized(new { message = "INVALID_APP_KEY" });

                // 3️⃣ Validate timestamp window (anti replay)
                long tsClient = long.Parse(timestamp);
                long tsServer = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                int allowedWindow = int.Parse(_config["Security:RequestTimeWindowSeconds"] ?? "60");

                if (Math.Abs(tsServer - tsClient) > allowedWindow * 1000)
                    return Unauthorized(new { message = "REQUEST_EXPIRED" });

                // 4️⃣ Validate HMAC signature
                string hmacKey = _config["Security:HmacSecret"];

                string raw = $"{model.data}|{timestamp}|{deviceId}";
                string expectedSignature;

                using (var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(hmacKey)))
                {
                    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(raw));
                    expectedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();
                }

                if (signature != expectedSignature)
                    return Unauthorized(new { message = "INVALID_SIGNATURE" });

                // 5️⃣ AES decrypt
                string aesKey = _config["Security:ClientEncryptionKey"];
                string json = AesEncryptionHelper.Decrypt(model.data, aesKey);

                var login = JsonSerializer.Deserialize<DTOLogin>(json);
                if (login == null)
                    return BadRequest("INVALID_LOGIN_DATA");

                // 6️⃣ Validate user
                var user = await loginRepository.Login(login);
                if (user == null)
                    return Unauthorized("Invalid credentials");

                // 7️⃣ Create JWT
                var jwt = _config.GetSection("Jwt");
                var jti = Guid.NewGuid().ToString();

                var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim(ClaimTypes.NameIdentifier, user.INI.ToString()),
            new Claim(ClaimTypes.Name, user.NAME),
            new Claim("WORKING_BRANCH", user.WORKING_BRANCH.ToString()),
            new Claim("ALLOW_BR_SELECTION", user.ALLOW_BR_SELECTION)
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));
                var token = new JwtSecurityToken(
                    issuer: jwt["Issuer"],
                    audience: jwt["Audience"],
                    expires: DateTime.UtcNow.AddMinutes(20),
                    claims: claims,
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                // 8️⃣ Store single-session jti
                _userTokenStore[user.INI.ToString()] = jti;

                // 9️⃣ Success
                // Prepare normal response data
                var responseObject = new
                {
                    token = tokenString,
                    userDetails = new
                    {
                        user.INI,
                        user.NAME,
                        user.ALLOW_BR_SELECTION,
                        user.WORKING_BRANCH,
                        user.DESIGNATION,
                        user.USER_LAVEL

                    }
                };

                // Convert to JSON
                string jsonResponse = JsonSerializer.Serialize(responseObject);

                // Encrypt before sending
                string encryptedResponse = AesEncryptionHelper.Encrypt(jsonResponse, aesKey);

                return Ok(new { data = encryptedResponse });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
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
}
