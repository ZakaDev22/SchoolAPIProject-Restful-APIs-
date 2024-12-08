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


        [HttpPost("AddNewNumber", Name = "AddNewNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<phoneNumberDTO>> AddNewNumberAsync(phoneNumberDTO phoneDTO)
        {
            if (phoneDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }
            if (phoneDTO.PersonID < 0 || phoneDTO.PhoneTypeID <= 0 || string.IsNullOrEmpty(phoneDTO.Number))
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var number = new clsPhoneNumbers(phoneDTO, clsPhoneNumbers.enMode.AddNew);

            if (await number.SaveAsync())
            {
                return CreatedAtRoute("GetPhoneNumberByID", new { ID = number.ID }, number.phoneNumberDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Phone Number Was Not Save." });
            }

        }

        [HttpPut("UpdatePhoneNumber/{ID}", Name = "UpdatePhoneNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<phoneNumberDTO>> UpdatePhoneNumberAsync(int ID, phoneNumberDTO phoneDTO)
        {
            if (phoneDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }


            if (ID <= 0 || phoneDTO.PersonID < 0 || phoneDTO.PhoneTypeID <= 0 || string.IsNullOrEmpty(phoneDTO.Number))
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var number = await clsPhoneNumbers.GetByIDAsync(ID);

            if (number == null)
                return NotFound($"No number With {ID} Have Ben Found");

            number.Number = phoneDTO.Number;
            number.PersonID = phoneDTO.PersonID;
            number.PhoneTypeID = phoneDTO.PhoneTypeID;
            number.IsPrimary = phoneDTO.IsPrimary;

            if (await number.SaveAsync())
            {
                return Ok($"Success, number With ID {number.ID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, number Was Not Save." });
            }

        }
    }


}
