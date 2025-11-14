using Microsoft.AspNetCore.Mvc;
using Models;
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var states = await _StateMaster.GetAll();
                return Ok(states);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching states: {ex.Message}");
            }
        }

        [HttpGet("GetStateById")] 
        public async Task<ActionResult> GetStateById(int stateCode, int countryCode)
        {
            try
            {
                var States = await _StateMaster.GetStateById(stateCode, countryCode);
                return Ok(States);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetState")] 
        public async Task<ActionResult> GetState( int countryCode)
        {
            try
            {
                var States = await _StateMaster.GetState( countryCode);
                return Ok(States);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] DTOStateMaster state)
        {
            if (state == null || string.IsNullOrWhiteSpace(state.Name))
                return BadRequest("Country name cannot be empty.");

            try
            {
                await _StateMaster.Save(state);
                return Ok(new { message = "State saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving State: {ex.Message}");
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] DTOStateMaster state)
        {
            if (state == null || state.Code <= 0) return BadRequest("Invalid State ID.");

            try
            {
                await _StateMaster.Update(state);
                return Ok(new { message = "State updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating State: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{stateCode}")]
        public async Task<IActionResult> Delete(long stateCode, int countryCode)
        {
            try
            {
                await _StateMaster.Delete(stateCode, countryCode);
                return Ok(new { message = "State Deleted Successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting state:{ex.Message}");
            }

        }
    }
}
