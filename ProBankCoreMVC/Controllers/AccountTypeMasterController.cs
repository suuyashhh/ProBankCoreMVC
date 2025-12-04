using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountTypeMasterController : Controller
    {
        private readonly IAccountTypeMaster _accountTypeMaster;
        public AccountTypeMasterController(IAccountTypeMaster accountTypeMaster)
        {
            _accountTypeMaster = accountTypeMaster;
        }

        [HttpGet("GetAllAccountType")]
        public async Task<ActionResult> GetAllAccountType()
        {
            try
            {
                var result = await _accountTypeMaster.GetAllAccountType();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetAccountTypeById")]
        public async Task<ActionResult> GetAccountTypeById(int Code)
        {
            try
            {
                var AccountType = await _accountTypeMaster.GetAccountTypeById(Code);
                return Ok(AccountType);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //[HttpGet("GetStaff")]
        //public async Task<ActionResult> GetStaff(int code)
        //{
        //    try
        //    {
        //        var Staff = await _staffMaster.GetStaff(code);
        //        return Ok(Staff);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] DTOAccountTypeMaster AccountType)
        {
            if (AccountType == null || string.IsNullOrWhiteSpace(AccountType.Name))
                return BadRequest("Account Type name cannot be empty.");

            try
            {
                await _accountTypeMaster.Save(AccountType);
                return Ok(new { message = "Account Type saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving Account Type: {ex.Message}");
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] DTOAccountTypeMaster AccountType)
        {
            if (AccountType == null || AccountType.Code <= 0) return BadRequest("Invalid Account Type ID.");

            try
            {
                await _accountTypeMaster.Update(AccountType);
                return Ok(new { message = "Account Type updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating AccountType: {ex.Message}");
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long Code)
        {
            try
            {
                await _accountTypeMaster.Delete(Code);
                return Ok(new { message = "AccountType deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting AccountType: {ex.Message}");
            }




        }
    }
}
