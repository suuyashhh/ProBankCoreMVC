using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;

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
                var createdProperty = _partyMaster.save(partymaster);
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
