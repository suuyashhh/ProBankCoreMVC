using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateMasterController : Controller
    {
        private readonly IStateMaster _StateMaster;

        public StateMasterController(IStateMaster stateMaster)
        {
            _StateMaster = stateMaster;
        }

        [HttpGet("GetStateById")]

        public async Task<ActionResult> GetStateById(int stateCode)
        {
            try
            {
                var States =await _StateMaster.GetStateById(stateCode);
                return Ok(States);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
