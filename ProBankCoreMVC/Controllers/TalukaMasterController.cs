using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TalukaMasterController : Controller
    {
        private readonly ITalukaMaster _talukaMaster;

        public TalukaMasterController(ITalukaMaster talukaMaster)
        {
            _talukaMaster = talukaMaster;
        }

        [HttpGet("GetTalukaById")]
        public async Task<ActionResult> GetTalukaById(int talukaCode)
        {
            try
            {
                var Talukas = await _talukaMaster.GetTalukaById(talukaCode);
                return Ok(Talukas);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetTaluka")]
        public async Task<ActionResult> GetTaluka(int Dist_code,int State_Code, int Country_Code)
        {
            try
            {
                var Talukas = await _talukaMaster.GetTaluka(Dist_code, State_Code, Country_Code);
                return Ok(Talukas);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var taluka = await _talukaMaster.GetAll();
                return Ok(taluka);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching taluka: {ex.Message}");
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] DTOTalukaMaster taluka)
        {
            if (taluka == null || string.IsNullOrWhiteSpace(taluka.name))
                return BadRequest("taluka name cannot be empty.");

            try
            {
                await _talukaMaster.Save(taluka);
                return Ok(new { message = "Taluka saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving taluka: {ex.Message}");
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] DTOTalukaMaster taluka)
        {
            if (taluka == null || taluka.Code <= 0) return BadRequest("Invalid taluka ID.");

            try
            {
                await _talukaMaster.Update(taluka);
                return Ok(new { message = "taluka updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating taluka: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{Code}")]
        public async Task<IActionResult> Delete(long Code, int Dist_code, int State_Code, int Country_Code)
        {
            try
            {
                await _talukaMaster.Delete(Code, Dist_code, State_Code, Country_Code);
                return Ok(new { message = " taluka deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting taluka: {ex.Message}");
            }
        }

    }
}
