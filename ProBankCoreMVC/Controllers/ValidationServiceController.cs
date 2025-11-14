// ValidationServiceController.cs
using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationServiceController : ControllerBase
    {
        private readonly IValidationService _validationService;
        public ValidationServiceController(IValidationService validationService) => _validationService = validationService;

        // call like: GET /api/ValidationService/AadharNo?aadharNo=123456789012
        [HttpGet("AadharNo")]
        public async Task<ActionResult<bool>> AadharNo([FromQuery] string aadharNo)
        {
            if (string.IsNullOrWhiteSpace(aadharNo))
                return BadRequest("aadharNo is required.");

            try
            {
                var exists = await _validationService.AadharNo(aadharNo);
                return Ok(exists);
            }
            catch (System.Exception ex)
            {
                // log ex
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
