using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;
using System.Data;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReligionMasterController : Controller
    {
        private readonly IReligionMaster _religionMaster;

        public ReligionMasterController(IReligionMaster religionMaster)
        {
            _religionMaster = religionMaster;
        }

        [HttpGet("GetAllReligion")]
        public async Task<ActionResult> GetAllReligion()
        {
            try
            {
                var result = await _religionMaster.GetAllReligion();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
