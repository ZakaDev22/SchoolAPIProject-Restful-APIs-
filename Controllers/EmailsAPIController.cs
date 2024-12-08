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
        public async Task<ActionResult<IEnumerable<classDTO>>> GetAllEmailsAsync()
        {
            var classes = await clsClasses.GetAllAsync();

            if (classes.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(classes);
        }
    }
}
