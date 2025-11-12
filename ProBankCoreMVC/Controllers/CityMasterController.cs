using Microsoft.AspNetCore.Mvc;
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
                // Optional: log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
