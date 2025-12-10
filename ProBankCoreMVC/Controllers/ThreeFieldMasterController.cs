using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThreeFieldMasterController : Controller
    {

        private readonly IThreeFieldMaster _threeFieldMaster;
        public ThreeFieldMasterController(IThreeFieldMaster threeFieldMaster)
        {
            _threeFieldMaster = threeFieldMaster;
        }

        [HttpGet("GetAllThreeFieldMaster")]
        public async Task<IActionResult> GetAllThreeFieldMaster(string tblName)
        {
            try
            {
                var masters = await _threeFieldMaster.GetAllThreeFieldMaster(tblName);

                return Ok(masters);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching data: {ex.Message}");
            }
        }

        [HttpGet("GetThreeFieldMasterById")]
        public async Task<IActionResult> GetThreeFieldMasterById(string tblName, int code)
        {
            try
            {
                var threeFieldMaster = await _threeFieldMaster.GetThreeFieldMasterById(tblName, code);

                if (threeFieldMaster == null)
                    return NotFound();

                return Ok(threeFieldMaster);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching data: {ex.Message}");
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] DTOThreeFieldMaster dto)
        {
            if (dto == null)
                return BadRequest("Request body is null.");

            // Basic validation: ensure table and column names are provided
            if (string.IsNullOrWhiteSpace(dto.TblName))
                return BadRequest("TblName is required.");

            if (string.IsNullOrWhiteSpace(dto.fld1))
                dto.fld1 = "CODE";      // sensible default
            if (string.IsNullOrWhiteSpace(dto.fld2))
                dto.fld2 = "NAME";
            if (string.IsNullOrWhiteSpace(dto.fld3))
                dto.fld3 = "ENTRY_DATE";

            try
            {
                // Call repository Save which will call the stored procedure.
                await _threeFieldMaster.Save(dto);

                // For insert (flag == 1) the repository/SP will populate dto.fld1val with generated CODE (if you applied the OUTPUT change)
                // Return Ok with the dto (including returned Msg and fld1val). Optionally, return CreatedAtAction if you have GetById.
                return Ok(dto);
            }
            catch (Exception ex)
            {
                // Log exception here if you have logging (not shown)
                return StatusCode(500, new { error = "An error occurred while saving.", detail = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] DTOThreeFieldMaster dto)
        {
            if (dto == null)
                return BadRequest("Request body is null.");

            if (string.IsNullOrWhiteSpace(dto.TblName))
                return BadRequest("TblName is required.");

            if (string.IsNullOrWhiteSpace(dto.fld1val))
                return BadRequest("fld1val (code) is required for update.");

            try
            {
                var updated = await _threeFieldMaster.Update(dto);

                // Evaluate message if you use numeric codes; return appropriate status
                // Typical behavior: return 200 OK with updated DTO and message.
                return Ok(updated);
            }
            catch (Exception ex)
            {
                // log ex
                return StatusCode(500, new { error = "Failed to update record.", detail = ex.Message });
            }
        }

        [HttpDelete("{tableName}/{code}")]
        public async Task<IActionResult> Delete(string tableName, string code)
        {
            if (string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(code))
                return BadRequest("tableName and code are required.");

            try
            {
                var result = await _threeFieldMaster.Delete(tableName, code);

                // check result.Msg for success/failure; here we assume SP returns '5' for delete success
                return Ok(result);
            }
            catch (Exception ex)
            {
                // log ex
                return StatusCode(500, new { error = "Failed to delete record.", detail = ex.Message });
            }
        }

    }
}
