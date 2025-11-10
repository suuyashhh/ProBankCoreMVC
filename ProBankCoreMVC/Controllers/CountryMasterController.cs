using Microsoft.AspNetCore.Mvc;
using Models;
using ProBankCoreMVC.Interfaces;
using System;

namespace ProBankCoreMVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryMasterController : Controller
    {
        private readonly ICountryMaster _countryMaster;

        public CountryMasterController(ICountryMaster countryMaster)
        {
            _countryMaster = countryMaster;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var countries = await _countryMaster.GetAll();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching countries: {ex.Message}");
            }
        }

        [HttpGet("GetCountryById")]
        public async Task<IActionResult> GetCountryById(long countryCode)
        {
            try
            {
                var country = await _countryMaster.GetCountryById(countryCode);
                if (country == null) return NotFound("Country not found.");
                return Ok(country);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while fetching country: {ex.Message}");
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] DTOCountryMaster country)
        {
            if (country == null || string.IsNullOrWhiteSpace(country.COUNTRY_NAME))
                return BadRequest("Country name cannot be empty.");

            try
            {
                await _countryMaster.Save(country);
                return Ok(new { message = "Country saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while saving country: {ex.Message}");
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] DTOCountryMaster country)
        {
            if (country == null || country.TRN_NO <= 0) return BadRequest("Invalid country ID.");

            try
            {
                await _countryMaster.Update(country);
                return Ok(new { message = "Country updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating country: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{countryCode}")]
        public async Task<IActionResult> Delete(long countryCode)
        {
            try
            {
                await _countryMaster.Delete(countryCode);
                return Ok(new { message = "Country deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting country: {ex.Message}");
            }
        }
    }
}
