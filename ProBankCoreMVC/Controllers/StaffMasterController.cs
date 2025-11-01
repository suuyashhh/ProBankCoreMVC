using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffMasterController : Controller
    {
        private readonly IStaffMaster _staffMaster;
        public StaffMasterController(IStaffMaster staffMaster)
        {
            _staffMaster = staffMaster;
        }

        [HttpGet("GetAllStaff")]
        public async Task<ActionResult> GetAllStaff()
        {
            try
            {
                var result = await _staffMaster.GetAllStaff();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
