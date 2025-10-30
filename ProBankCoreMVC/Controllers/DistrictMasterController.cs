using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> GetDistrictById(int countryCode, int stateCode, int distCode)
        {
            try
            {
                var Districts = await _districtMaster.GetDistrictById(countryCode, stateCode, distCode);
                return Ok(Districts);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
