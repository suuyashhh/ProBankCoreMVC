using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProBankCoreMVC.Interfaces;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidationServiceController : ControllerBase
    {
        private readonly IValidationService _validationService;
        public ValidationServiceController(IValidationService validationService) => _validationService = validationService;

        [HttpGet("AadharNo")]
        public async Task<ActionResult<bool>> AadharNo([FromQuery] string aadharNo)
        {
            if (string.IsNullOrWhiteSpace(aadharNo))
                return BadRequest("aadharNo is required.");

            try
            {
                var result = await _validationService.AadharNo(aadharNo);
                var data = new
                {
                    exist = result
                };
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("PanNo")]
        public async Task<ActionResult<bool>> PanNo([FromQuery] string panNo)
        {
            if (string.IsNullOrWhiteSpace(panNo))
                return BadRequest("PanNo is required.");

            try
            {
                var result = await _validationService.PanNo(panNo);
                var data = new
                {
                    exist = result
                };

                return Ok(data);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("GstNo")]
        public async Task<ActionResult<bool>> GstNo([FromQuery] string gstNo)
        {
            if (string.IsNullOrWhiteSpace(gstNo))
                return BadRequest("GstNo is required. ");

            try
            {
                var result = await _validationService.GstNo(gstNo);
                var data = new
                {
                    exist = result
                };
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("MobileNo")]
        public async Task<ActionResult<bool>> MobileNo([FromQuery] string mobileNo)
        {
            if (string.IsNullOrWhiteSpace(mobileNo))
                return BadRequest("MobileNo is required. ");

            try
            {
                var result = await _validationService.MobileNo(mobileNo);
                var data = new
                {
                    exist = result
                };
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("PhoneNo")]
        public async Task<ActionResult<bool>> PhoneNo([FromQuery] string phone1)
        {
            if (string.IsNullOrWhiteSpace(phone1))
                return BadRequest("PhoneNo is required.");

            try
            {
                var result = await _validationService.PhoneNo(phone1);
                var data = new
                {
                    exist = result
                };
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("VoterIdNo")]
        public async Task<ActionResult<bool>> VoterIdNo([FromQuery] string voterIdNo)
        {
            if (string.IsNullOrWhiteSpace(voterIdNo))
                return BadRequest("VoterIdNo is required.");

            try
            {
                var result = await _validationService.VoterIdNo(voterIdNo);
                var data = new
                {
                    exist = result
                };
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("PassportNo")]
        public async Task<ActionResult<bool>> PassportNo([FromQuery] string passportNo)
        {
            if (string.IsNullOrWhiteSpace(passportNo))
                return BadRequest("PassportNo is required.");

            try
            {
                var result = await _validationService.PassportNo(passportNo);
                var data = new
                {
                    exist = result
                };
                return Ok(data);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }

    
}
