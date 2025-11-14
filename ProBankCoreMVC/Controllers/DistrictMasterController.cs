using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistrictMasterController : Controller
    {
        private readonly IDistrictMaster _districtMaster;

        public DistrictMasterController(IDistrictMaster districtMaster)
        {
            _districtMaster = districtMaster;
        }

        [HttpGet("GetDistrictById")]
        public async Task<ActionResult> GetDistrictById(int distCode, int Country_Code, int State_Code)
        {
            try
            {
                var Districts = await _districtMaster.GetDistrictById(distCode, Country_Code, State_Code);
                return Ok(Districts);
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
                var districts = await _districtMaster.GetAll();
                return Ok(districts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching district: {ex.Message}");
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] DTODistrictMaster district)
        {
            if (district == null || string.IsNullOrWhiteSpace(district.Name))
                return BadRequest("District name cannot be empty.");

            try
            {
                await _districtMaster.Save(district);
                return Ok(new { message = "district saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving district: {ex.Message}");
            }
        }


        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] DTODistrictMaster district)
        {
            if (district == null || district.Code <= 0) return BadRequest("Invalid district ID.");

            try
            {
                await _districtMaster.Update(district);
                return Ok(new { message = "district updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating district: {ex.Message}");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long Code, int State_Code, int Country_Code)
        {
            try
            {
                await _districtMaster.Delete(Code, State_Code, Country_Code);
                return Ok(new { message = "district deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting district: {ex.Message}");
            }
        }

    }
}
