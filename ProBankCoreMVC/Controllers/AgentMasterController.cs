using Microsoft.AspNetCore.Mvc;
using Models;
using Pipelines.Sockets.Unofficial.Arenas;
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

        [HttpGet("GetAgentById")]
        public async Task<ActionResult> GetAgentById(int ID)
        {
            try
            {
                var agent = await _agentMaster.GetAgentById(ID);

                if (agent == null)
                    return NotFound();

                return Ok(agent);
            }
            catch (Exception ex)
            {
                // log ex if needed
                return StatusCode(500, $"Error fetching agent: {ex.Message}");
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] DTOAgentMaster Agent)
        {
            if (Agent == null || string.IsNullOrWhiteSpace(Agent.NAME))
                return BadRequest("Agent name cannot be empty.");

            try
            {
                // 🔑 CALL SAVE ONLY ONCE
                var newAgentCode = await _agentMaster.Save(Agent);

                return Ok(new
                {
                    message = "Agent saved successfully.",
                    agentCode = newAgentCode
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving Agent: {ex.Message}");
            }
        }


        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] DTOAgentMaster Agent)
        {
            if (Agent == null || Agent.ID <= 0)
                return BadRequest("Invalid Agent ID.");

            try
            {
                await _agentMaster.Update(Agent);
                return Ok(new { message = "Agent updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating agent: {ex.Message}");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int ID)
        {
            try
            {
                await _agentMaster.Delete(ID);
                return Ok(new { message = "Agent deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting Agent: {ex.Message}");
            }
        }


    }
}
