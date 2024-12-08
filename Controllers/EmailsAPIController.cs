using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/EmailsAPI")]
    [ApiController]
    public class EmailsAPIController : ControllerBase
    {
        [HttpGet("GetAllEmails", Name = "GetAllEmails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EmailDTO>>> GetAllEmailsAsync()
        {
            var emails = await clsEmails.GetAllAsync();

            if (emails.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(emails);
        }

        [HttpGet("GetEmailByID/{ID}", Name = "GetEmailByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<classDTO>>> GetEmailByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var email = await clsEmails.GetByIDAsync(ID);

            if (email == null)
                return NotFound($"No email With ID {ID} Is Not Found!");


            return Ok(email.emailDTO);
        }

        [HttpGet("IsEmailExistsByID/{ID}", Name = "IsEmailExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsEmailExistsByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsEmails.IsExistsAsync(ID))
                return NotFound($"No Email With ID {ID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpDelete("DeleteEmailByID/{ID}", Name = "DeleteEmailByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteEmailByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");


            if (!await clsEmails.IsExistsAsync(ID))
                return NotFound($"No Email With ID {ID} Has Ben  Found!");


            if (await clsEmails.DeleteAsync(ID))
                return Ok($"Success, EMail With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewEmail", Name = "AddNewEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmailDTO>> AddNewEMailAsync(EmailDTO emailDTO)
        {
            if (emailDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (emailDTO.PersonID < 0 || emailDTO.EmailTypeID <= 0 || string.IsNullOrEmpty(emailDTO.Address))
            {
                return base.BadRequest(" Some DTO Properties Are Empty!");
            }

            var email = new clsEmails(emailDTO, clsEmails.enMode.AddNew);

            if (await email.SaveAsync())
            {
                return CreatedAtRoute("GetEmailByID", new { ID = email.ID }, email.emailDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, EMail Was Not Save." });
            }

        }

        [HttpPut("UpdateEmail/{ID}", Name = "UpdateEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmailDTO>> UpdateEMailAsync(int ID, EmailDTO emailDTO)
        {
            if (emailDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }


            if (ID <= 0 || emailDTO.PersonID < 0 || emailDTO.EmailTypeID <= 0 || string.IsNullOrEmpty(emailDTO.Address))
            {
                return base.BadRequest(" Some DTO Properties Are Empty!");
            }

            var email = await clsEmails.GetByIDAsync(ID);

            if (email == null)
                return NotFound($"No email With {ID} Have Ben Found");

            email.Address = emailDTO.Address;
            email.PersonID = emailDTO.PersonID;
            email.EmailTypeID = emailDTO.EmailTypeID;
            email.IsPrimary = emailDTO.IsPrimary;

            if (await email.SaveAsync())
            {
                return Ok($"Success, email With ID {email.ID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, email Was Not Save." });
            }

        }
    }
}
