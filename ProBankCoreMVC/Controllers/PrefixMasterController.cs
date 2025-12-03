using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using ProBankCoreMVC.Repositries;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrefixMasterController : Controller
    {
        private readonly IPrefixMaster _prefixMaster;
        public PrefixMasterController(IPrefixMaster prefixMaster)
        {
            _prefixMaster = prefixMaster;
        }

        [HttpGet("GetAllPrefix")]
        public async Task<ActionResult> GetAllPrefix()
        {
            try
            {
                var result = await _prefixMaster.GetAllPrefix();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] DTOPrefixMaster Prefix)
        {
            if (Prefix == null || string.IsNullOrWhiteSpace(Prefix.Prefixtype))
                return BadRequest("Prefix name cannot be empty.");

            try
            {
                await _prefixMaster.Save(Prefix);
                return Ok(new { message = "Prefix saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving Prefix: {ex.Message}");
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] DTOPrefixMaster Prefix)
        {
            if (Prefix == null || Prefix.Code <= 0) return BadRequest("Invalid Prefix ID.");

            try
            {
                await _prefixMaster.Update(Prefix);
                return Ok(new { message = "Prefix updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating Prefix: {ex.Message}");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long Code)
        {
            try
            {
                await _prefixMaster.Delete(Code);
                return Ok(new { message = "Prefix deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting Prefix: {ex.Message}");
            }




        }


    }
}
