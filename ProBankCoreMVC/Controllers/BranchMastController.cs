using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchMastController : ControllerBase
    {
        private readonly IBranchMast _branchMast;

        public BranchMastController(IBranchMast branchMast)
        {
            _branchMast = branchMast;
        }

        [HttpGet("GetAllBranches")]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var branches = await _branchMast.GetAllBranches();
                return Ok(branches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching branches.");
            }
        }
    }
}
