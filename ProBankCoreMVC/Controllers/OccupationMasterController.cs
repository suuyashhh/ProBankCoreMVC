using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OccupationMasterController : Controller
    {
        private readonly IOccupationMaster _occupationMaster;
        public OccupationMasterController(IOccupationMaster occupationMaster)
        {
            _occupationMaster = occupationMaster;
        }

        [HttpGet("GetAllOccupations")]
        public async Task<ActionResult> GetAllOccupations()
        {
            try
            {
                var result = await _occupationMaster.GetAllOccupations();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
