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

        [HttpGet("IsPhoneNumberExistsByID/{ID}", Name = "IsPhoneNumberExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsPhoneNumberExistsByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsPhoneNumbers.IsExistsAsync(ID))
                return NotFound($"No Phone Number With ID {ID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpDelete("DeletePhoneNumberByID/{ID}", Name = "DeletePhoneNumberByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeletePhoneNumberByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");


            if (!await clsPhoneNumbers.IsExistsAsync(ID))
                return NotFound($"No Phone Number With ID {ID} Has Ben  Found!");


            if (await clsPhoneNumbers.DeleteAsync(ID))
                return Ok($"Success, Phone Number With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


}
