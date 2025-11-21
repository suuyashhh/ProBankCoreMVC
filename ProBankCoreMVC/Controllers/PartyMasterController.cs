using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;
using static Models.DTOPartyMaster;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartyMasterController : Controller
    {
        private readonly IPartyMaster _partyMaster;

        public PartyMasterController(IPartyMaster partyMaster)
        {
            _partyMaster = partyMaster;
        }

        [HttpGet("GetCustomers")]
        public async Task<ActionResult<List<DTOPartyMaster.CustomerSummary>>> GetCustomers(int branchCode, string? search = null)
        {
            try
            {
                var result = await _partyMaster.GetCustomers(branchCode, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // TODO: log ex properly
                return StatusCode(500, "Failed to load customers");
            }
        }


        [HttpPost]
        [Route("save")]
        public async Task<ActionResult<DTOPartyMaster>> save(DTOPartyMaster partymaster)

        {
            try
            {
                if (partymaster == null)
                {
                    return BadRequest();
                }
                var createdProperty = _partyMaster.Save(partymaster);
                var result = new
                {
                    data = partymaster,
                    Message = "success"
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving Data");
            }
        }
    }
}
