using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;
using Models;
using System;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _loginRepository;
        private readonly IAuthService _authService;

        public LoginController(ILogin loginRepository, IAuthService authService)
        {
            _loginRepository = loginRepository;
            _authService = authService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] DTOLogin login)
        {
            if (login == null || string.IsNullOrEmpty(login.INI) || string.IsNullOrEmpty(login.CODE))
                return BadRequest("ini and Code are required.");

            try
            {
                var result = await _authService.AuthenticateAndCreateTokenAsync(login.INI, login.CODE);
                return Ok(new { token = result.token, expires = result.expires, user = login.INI });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                // generic server error
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
