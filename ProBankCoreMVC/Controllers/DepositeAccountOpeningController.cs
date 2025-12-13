using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepositeAccountOpeningController : Controller
    {
        private readonly IDepositeAccountOpening _depositAccOpe;

        public DepositeAccountOpeningController(IDepositeAccountOpening depositAccOpe)
        {
            _depositAccOpe = depositAccOpe;
        }

        [HttpGet("GetGlCodeAll")]
        public async Task<ActionResult> GetGlCodeAll() 
        {
            var result = await _depositAccOpe.GetGlCodeAll();
            return Ok(result);
        }

    }
}
