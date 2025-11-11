using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DireMastController : Controller
    {
        private readonly IDireMast _direMast;
        public DireMastController(IDireMast direMast)
        {
            _direMast = direMast;
        }

        [HttpGet("GetAllOther")]
        public async Task<ActionResult> GetAllOther()
        {
            try
            {
                var result = await _direMast.GetAllOther();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
