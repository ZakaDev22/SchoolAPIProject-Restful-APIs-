using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/CountriesAPI")]
    [ApiController]
    public class CountriesAPIController : ControllerBase
    {
        [HttpGet("GetAllCountries", Name = "GetAllCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<countryDTO>>> GetAllCountriesAsync()
        {
            var countries = await clsCountries.GetAllAsync();

            if (countries.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(countries);
        }

        [HttpGet("GetCountryByID/{ID}", Name = "GetCountryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<countryDTO>>> GetCountryByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var country = await clsCountries.GetByIDAsync(ID);

            if (country == null)
                return NotFound($"No country With ID {ID} Is Not Found!");


            return Ok(country.countryDTO);
        }

        [HttpGet("GetCountryByName/{CountryID}", Name = "GetCountryByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<countryDTO>>> GetCountryByNameAsync(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return BadRequest($"Invalid Data !");

            var country = await clsCountries.GetByCountryNameAsync(Name);

            if (country == null)
                return NotFound($"No country With CountryID {Name} Is Not Found!");


            return Ok(country.countryDTO);
        }

        [HttpGet("GetCountryByCode/{Name}", Name = "GetCountryByCode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<countryDTO>>> GetCountryByCodeAsync(string Code)
        {
            if (string.IsNullOrEmpty(Code))
                return BadRequest($"Invalid Data !");

            var country = await clsCountries.GetByCountryNameAsync(Code);

            if (country == null)
                return NotFound($"No country With CountryID {Code} Is Not Found!");


            return Ok(country.countryDTO);
        }
    }
}
