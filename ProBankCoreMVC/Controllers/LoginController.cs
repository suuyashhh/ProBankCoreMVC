using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;
using Models;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _loginRepository;

        public LoginController(ILogin loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] DTOLogin login)
        {
            if (login == null || string.IsNullOrEmpty(login.INI) || string.IsNullOrEmpty(login.CODE))
                return BadRequest("ini and Code are required.");

            var isValid = await _loginRepository.ValidateUserAsync(login.INI, login.CODE);

            return Ok(isValid); 
        }
    }
}
