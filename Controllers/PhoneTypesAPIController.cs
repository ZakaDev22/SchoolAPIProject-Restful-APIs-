using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/PhoneTypesAPI")]
    [ApiController]
    public class PhoneTypesAPIController : ControllerBase
    {
        [HttpGet("GetAllPhoneTypes", Name = "GetAllPhoneTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<phoneTypesDTO>>> GetAllPhoneTypesAsync()
        {
            var phoneTypes = await clsPhoneTypes.GetAllAsync();

            if (phoneTypes.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(phoneTypes);
        }

        [HttpGet("GetPhoneTypeByID/{ID}", Name = "GetPhoneTypeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<phoneTypesDTO>>> GetPhoneTypeByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var phoneType = await clsPhoneTypes.GetByIDAsync(ID);

            if (phoneType is null)
                return NotFound($"No phone Type With ID {ID} Is Not Found!");


            return Ok(phoneType.phoneTypeDTO);
        }

        [HttpDelete("DeletePhoneTypeByID/{ID}", Name = "DeletePhoneTypeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeletePhoneTypeByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var type = await clsPhoneTypesData.GetByIdAsync(ID);

            if (type is null)
                return NotFound($"No phone Type With ID {ID} Has Ben  Found!");


            if (await clsPhoneTypes.DeleteAsync(ID))
                return Ok($"Success, Phone Type With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
