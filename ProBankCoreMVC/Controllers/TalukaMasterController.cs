using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TalukaMasterController : Controller
    {
        private readonly ITalukaMaster _talukaMaster;

        public TalukaMasterController(ITalukaMaster talukaMaster)
        {
            _talukaMaster = talukaMaster;
        }

        [HttpGet("GetTalukaById")]
        public async Task<ActionResult> GetTalukaById(int talukaCode)
        {
            try
            {
                var Talukas = await _talukaMaster.GetTalukaById( talukaCode);
                return Ok(Talukas);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
