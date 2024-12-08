using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/PhoneNumbersAPI")]
    [ApiController]
    public class PhoneNumbersAPIController : ControllerBase
    {
        [HttpGet("GetAllPhoneNumbers", Name = "GetAllPhoneNumbers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<phoneNumberDTO>>> GetAllPhoneNumbersAsync()
        {
            var numbers = await clsPhoneNumbers.GetAllAsync();

            if (numbers.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(numbers);
        }

        [HttpGet("GetPhoneNumberByID/{ID}", Name = "GetPhoneNumberByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<phoneNumberDTO>>> GetPhoneNumberByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var number = await clsPhoneNumbers.GetByIDAsync(ID);

            if (number == null)
                return NotFound($"No number With ID {ID} Is Not Found!");


            return Ok(number.phoneNumberDTO);
        }
    }
}
