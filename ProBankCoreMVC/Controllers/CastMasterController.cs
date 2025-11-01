using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;
using System.ComponentModel;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CastMasterController : Controller
    {
        private readonly ICastMaster _castMaster;
        public CastMasterController(ICastMaster castMaster)
        {
            _castMaster = castMaster;
        }

        [HttpGet("GetAllCast")]
        public async Task<ActionResult> GetAllCast()
        {
            try
            {
                var result = await _castMaster.GetAllCast();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
