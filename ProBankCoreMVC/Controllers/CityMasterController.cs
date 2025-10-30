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

        [HttpGet("GetAllCities")]
        public async Task<ActionResult> GetAllCities()
        {
            try
            {
                var Cities = await _cityMaster.GetAllCities();
                return Ok(Cities);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetDependencyByCityId")]

        public async Task<ActionResult> GetDependencyByCityId(int cityUnicId)
        {
            try
            {
                var result = await _cityMaster.GetDependencyByCityId(cityUnicId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
