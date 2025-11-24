// ProBankCoreMVC/Controllers/UserMenuAccessController.cs
using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserMenuAccessController : Controller
    {
        private readonly IUserMenuAccess _userMenuAccess;

        public UserMenuAccessController(IUserMenuAccess userMenuAccess)
        {
            _userMenuAccess = userMenuAccess;
        }

        [HttpGet("UserGrades")]
        public async Task<ActionResult> GetUserGrades()
        {
            try
            {
                var grades = await _userMenuAccess.GetUserGradesAsync();
                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching user grades: {ex.Message}");
            }
        }

        [HttpGet("MenuMaster")]
        public async Task<ActionResult> GetMenuMaster(int programeId = 1)
        {
            try
            {
                var menus = await _userMenuAccess.GetMenuMasterAsync(programeId);
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching menu master: {ex.Message}");
            }
        }

        [HttpGet("SelectedMenus")]
        public async Task<ActionResult> GetSelectedMenus(long userGrad, int programeId = 1)
        {
            if (userGrad <= 0)
                return BadRequest("Invalid user grade.");

            try
            {
                var selectedMenuIds = await _userMenuAccess.GetSelectedMenuIdsAsync(userGrad, programeId);
                return Ok(selectedMenuIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching selected menus: {ex.Message}");
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] DTOUserMenuAccess model)
        {
            if (model == null || model.UserGrad <= 0)
                return BadRequest("Invalid request.");

            try
            {
                await _userMenuAccess.SaveUserMenuAccessAsync(model);
                return Ok(new { message = "User menu access saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving user menu access: {ex.Message}");
            }
        }
    }
}
