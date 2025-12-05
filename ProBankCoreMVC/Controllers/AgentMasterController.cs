using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentMasterController : Controller
    {
        private readonly IAgentMaster _agentMaster;
        public AgentMasterController(IAgentMaster agentMaster)
        {
            _agentMaster = agentMaster;
        }

        [HttpGet("GetAllAgent")]
        public async Task<ActionResult> GetAllAgent()
        {
            try
            {
                var result = await _agentMaster.GetAllAgent();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
