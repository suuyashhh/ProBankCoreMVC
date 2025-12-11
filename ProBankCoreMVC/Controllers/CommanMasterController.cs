using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommanMasterController : Controller
    {


        private readonly ICommanMaster _commanMaster;
        public CommanMasterController(ICommanMaster commanMaster)
        {
            _commanMaster = commanMaster;
        }


        [HttpGet("GetAllMaster")]
        public async Task<IActionResult> GetAllMaster(string tblName)
        {
            try
            {
                var masters = await _commanMaster.GetAllMaster(tblName);

                return Ok(masters);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching data: {ex.Message}");
            }
        }



        [HttpGet("GetCommanMasterById")]
        public async Task<IActionResult> GetCommanMasterById(string tblName, int code)
        {
            try
            {
                var commanMaster = await _commanMaster.GetCommanMasterById(tblName, code);

                if (commanMaster == null)
                    return NotFound();

                return Ok(commanMaster);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching data: {ex.Message}");
            }
        }
             

        [HttpPost("Save")]
        public async Task<IActionResult> Save(string tblName,  string name)
        {

            try
            {
                await _commanMaster.Save( tblName,  name);
                return Ok(new { message = " saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving : {ex.Message}");
            }
        }

        [HttpPut("UpdateCommanMaster")]
        public async Task<IActionResult> UpdateCommanMaster(string tblName, int code, string name)
        {
            try
            {
                await _commanMaster.UpdateCommanMaster(tblName, code, name);
                return Ok(new { message = "Updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating: {ex.Message}");
            }
        }

        [HttpDelete("DeleteCommanMaster")]
        public async Task<IActionResult> DeleteCommanMaster(string tblName, int code)
        {
            try
            {
                await _commanMaster.DeleteCommanMaster(tblName, code);
                return Ok(new { message = "Deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting: {ex.Message}");
            }
        }
    }
}
