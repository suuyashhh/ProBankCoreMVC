using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryMasterController : Controller
    {
        private readonly ICountryMaster _countryMaster;

        public CountryMasterController(ICountryMaster countryMaster)
        {
            _countryMaster = countryMaster;
        }

        [HttpGet("GetCountryById")]
        public async Task<ActionResult> GetCountryById(int countryCode)
        {
            try
            {
                var Countries = await _countryMaster.GetCountryById(countryCode);
                return Ok(Countries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching Countries.");
            }
        }
    }
}
