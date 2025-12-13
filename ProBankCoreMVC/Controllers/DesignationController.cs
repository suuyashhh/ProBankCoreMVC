using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DesignationController : Controller
    {
        private readonly IDesignationMaster _designationMaster;
        public DesignationController(IDesignationMaster designationMaster)
        {
            _designationMaster = designationMaster;
        }

        [HttpGet("GetAllDesignations")]
        public async Task<ActionResult> GetAllDesignations()
        {
            try
            {
                var result = await _designationMaster.GetAllDesignations();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
