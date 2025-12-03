using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;
using System.Data;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReligionMasterController : Controller
    {
        private readonly IReligionMaster _religionMaster;

        public ReligionMasterController(IReligionMaster religionMaster)
        {
            _religionMaster = religionMaster;
        }

        [HttpGet("GetAllReligion")]
        public async Task<ActionResult> GetAllReligion()
        {
            try
            {
                var result = await _religionMaster.GetAllReligion();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetReligionById")]
        public async Task<ActionResult> GetReligionById(int code)
        {
            try
            {
                var religion = await _religionMaster.GetReligionById(code);
                return Ok(religion);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] DTOReligionMaster Religion)
        {
            if (Religion == null || string.IsNullOrWhiteSpace(Religion.RELIGION_NAME))
                return BadRequest("Religion name cannot be empty.");

            try
            {
                await _religionMaster.Save(Religion);
                return Ok(new { message = "Religion saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving Religion: {ex.Message}");
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] DTOReligionMaster Religion)
        {
            if (Religion == null || Religion.RELIGION_CODE <= 0) return BadRequest("Invalid Religion ID.");

            try
            {
                await _religionMaster.Update(Religion);
                return Ok(new { message = "religion updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating religion: {ex.Message}");
            }
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long Code)
        {
            try
            {
                await _religionMaster.Delete(Code);
                return Ok(new { message = "Religion deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting religion: {ex.Message}");
            }




        }


    }
}
