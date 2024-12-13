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
                return NotFound($"No type Type With ID {ID} Is Not Found!");


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

        [HttpPost("AddNewEmailType", Name = "AddNewEmailType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<emailTypesDTO>> AddNewEMailTypeAsync(emailTypesDTO emailDTO)
        {
            if (emailDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (string.IsNullOrEmpty(emailDTO.Name))
            {
                return base.BadRequest(" Some DTO Properties Are Empty!");
            }

            var type = new clsEmailsTypes(emailDTO, clsEmailsTypes.enMode.AddNew);

            if (await type.SaveAsync())
            {
                return CreatedAtRoute("GetEmailTypeByID", new { ID = type.ID }, type.emailTypeDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, EMail Type Was Not Save." });
            }

        }

        [HttpPut("UpdateEmailType/{ID}", Name = "UpdateEmailType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmailDTO>> UpdateEmailTypeAsync(int ID, emailTypesDTO emailDTO)
        {
            if (emailDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }


            if (string.IsNullOrEmpty(emailDTO.Name))
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var emailType = await clsEmailsTypes.GetByIDAsync(ID);

            if (emailType is null)
                return NotFound($"No type With {ID} Have Ben Found");

            emailType.Name = emailDTO.Name;


            if (await emailType.SaveAsync())
            {
                return Ok($"Success, type With ID {emailType.ID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, type Was Not Save." });
            }

        }
    }
}
