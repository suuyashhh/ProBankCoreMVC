using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityMasterController : Controller
    {
        private readonly ICityMaster _cityMaster;

        public CityMasterController(ICityMaster cityMaster)
        {
            _cityMaster = cityMaster;
        }

        [HttpGet("GetAllCity")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var result = await _cityMaster.GetAllCity();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetCityById")]
        public async Task<IActionResult> GetCityById(int country, int state, int dist, int taluka, int code)
        {
            try
            {

                var city = await _cityMaster.GetCityById(country, state, dist, taluka, code);
                if (city == null) return NotFound("City not found.");
                return Ok(city);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching cities: {ex.Message}");
            }
        }






        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] DTOCityMaster objList)
        {
            if (objList == null || string.IsNullOrWhiteSpace(objList.CITY_NAME))
                return BadRequest("City name cannot be empty.");


            try
            {
                await _cityMaster.Save(objList);
                return Ok(new { message = "City saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving citys: {ex.Message}");
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] DTOCityMaster objList)
        {
            if (objList == null || objList.CITY_CODE <= 0) return BadRequest("Invalid citys ID.");

            try
            {
                await _cityMaster.Update(objList);
                return Ok(new { message = "Country updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating citys: {ex.Message}");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int country, int state, int dist, int taluka, int code)
        {
            try
            {
                await _cityMaster.Delete(country, state, dist, taluka, code);
                return Ok(new { message = "city deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting citys: {ex.Message}");
            }
        }

        [HttpGet("GetAllDependencies")]
        public async Task<ActionResult> GetAllDependencies()
        {
            try
            {
                var result = await _cityMaster.GetAllDependencies();
                if (result == null || !result.Any())
                    return NotFound("No cities found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetDistrictsByState")]
        public async Task<IActionResult> GetDistrictsByState(int countryCode, int stateCode)
        {
            var data = await _cityMaster.GetDistrictsByState(countryCode, stateCode);
            return Ok(data);
        }

        [HttpGet("GetTalukasByDistrict")]
        public async Task<IActionResult> GetTalukasByDistrict(int countryCode, int stateCode, int distCode)
        {
            var data = await _cityMaster.GetTalukasByDistrict(countryCode, stateCode, distCode);
            return Ok(data);
        }

    }
}
