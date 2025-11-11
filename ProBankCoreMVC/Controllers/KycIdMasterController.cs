using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KycIdMasterController : Controller
    {
        private readonly IKycIdMaster _kycIdMaster;

        public KycIdMasterController(IKycIdMaster kycIdMaster)
        {
            _kycIdMaster = kycIdMaster;
        }

        [HttpGet("GetAllKycId")]
        public async Task<ActionResult> GetAllKycId()
        {
            try
            {
                var result = await _kycIdMaster.GetAllKycId();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
