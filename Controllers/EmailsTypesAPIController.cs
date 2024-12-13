using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/EmailsTypesAPI")]
    [ApiController]
    public class EmailsTypesAPIController : ControllerBase
    {
        [HttpGet("GetAllEmailTypes", Name = "GetAllEmailTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<emailTypesDTO>>> GetAllEmailTypesAsync()
        {
            var emailTypes = await clsEmailsTypesData.GetAllAsync();

            if (emailTypes.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(emailTypes);
        }

        [HttpGet("GetEmailTypeByID/{ID}", Name = "GetEmailTypeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<emailTypesDTO>>> GetEmailTypeByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var emailType = await clsEmailsTypes.GetByIDAsync(ID);

            if (emailType is null)
                return NotFound($"No email Type With ID {ID} Is Not Found!");


            return Ok(emailType.emailTypeDTO);
        }

        [HttpDelete("DeleteEmailTypeByID/{ID}", Name = "DeleteEmailTypeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteEmailTypeByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var type = await clsEmailsTypes.GetByIDAsync(ID);

            if (type is null)
                return NotFound($"No Email Type With ID {ID} Has Ben  Found!");


            if (await clsEmailsTypes.DeleteAsync(ID))
                return Ok($"Success, EMail Type With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
