using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AreaMasterController : Controller
    {
       private readonly IAreaMaster _areaMaster;

        public AreaMasterController(IAreaMaster areaMaster)
        {
            _areaMaster = areaMaster;
        }

        [HttpGet("GetAreaById")]
        public async Task<ActionResult> GetAreaById(int countryCode, int stateCode, int distCode, int talukaCode, int cityCode)
        {
            try
            {
                var result = await _areaMaster.GetAreaById(countryCode,stateCode, distCode, talukaCode,cityCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;

            }
        }
    }
}
