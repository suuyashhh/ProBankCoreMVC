using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffMasterController : Controller
    {
        private readonly IStaffMaster _staffMaster;
        public StaffMasterController(IStaffMaster staffMaster)
        {
            _staffMaster = staffMaster;
        }

        [HttpGet("GetAllStaff")]
        public async Task<ActionResult> GetAllStaff()
        {
            try
            {
                var result = await _staffMaster.GetAllStaff();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetStaffById")]
        public async Task<ActionResult> GetStaffById(int code)
        {
            try
            {
                var satff = await _staffMaster.GetStaffById(code);
                return Ok(satff);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetStaff")]
        public async Task<ActionResult> GetStaff(int code)
        {
            try
            {
                var Staff = await _staffMaster.GetStaff(code);
                return Ok(Staff);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] DTOStaffMaster Staff)
        {
            if (Staff == null || string.IsNullOrWhiteSpace(Staff.name))
                return BadRequest("Staff name cannot be empty.");

            try
            {
                await _staffMaster.Save(Staff);
                return Ok(new { message = "Staff saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving Staff: {ex.Message}");
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] DTOStaffMaster Staff)
        {
            if (Staff == null || Staff.code <= 0) return BadRequest("Invalid Staff ID.");

            try
            {
                await _staffMaster.Update(Staff);
                return Ok(new { message = "staff updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating staff: {ex.Message}");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long code)
        {
            try
            {
                await _staffMaster.Delete(code);
                return Ok(new { message = "staff deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting staff: {ex.Message}");
            }




        }





    }
}
