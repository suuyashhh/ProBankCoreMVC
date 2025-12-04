using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KycAddressMasterController : Controller
    {
        private readonly IKycAddressMaster _kycAddressMaster;
        public KycAddressMasterController(IKycAddressMaster kycAddressMaster)
        {
            _kycAddressMaster = kycAddressMaster;
        }

        [HttpGet("GetAllKycAddress")]
        public async Task<ActionResult> GetAllKycAddress()
        {
            try
            {
                var result = await _kycAddressMaster.GetAllKycAddress();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
