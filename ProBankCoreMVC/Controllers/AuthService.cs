using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProBankCoreMVC.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Controllers
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly ILogin _loginRepo;

        public AuthService(IConfiguration config, ILogin loginRepo)
        {
            _config = config;
            _loginRepo = loginRepo;
        }

        public async Task<(string token, DateTime expires)> AuthenticateAndCreateTokenAsync(string ini, string code)
        {
            // Validate credentials
            var isValid = await _loginRepo.ValidateUserAsync(ini, code);
            if (!isValid) throw new UnauthorizedAccessException("Invalid credentials");

            var userId = ini; // use ini as identifier; change if your PK is different

            var jwtSection = _config.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var minutes = jwtSection.GetValue<int>("ExpiryMinutes");
            var expires = DateTime.UtcNow.AddMinutes(minutes);

            var jti = Guid.NewGuid().ToString();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
            };

            var keyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(keyBytes, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Store jti for the user
            await _loginRepo.SetUserJtiAsync(userId, jti);

            return (tokenString, expires);
        }
    }
}
