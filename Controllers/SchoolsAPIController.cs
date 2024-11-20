using Microsoft.AspNetCore.Mvc;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/SchoolsAPI")]
    [ApiController]
    public class SchoolsAPIController : ControllerBase
    {
        [HttpGet("GetAllSchools", Name = "GetAllSchools")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<schoolDTO>>> GetAllScholls()
        {
            var schools = await clsSchools.GetAllAsync();

            if (schools == null)
                return NotFound("There Is No Data!");

            return Ok(schools);
        }

        [HttpGet("GetSchoolByID/{ID}", Name = "GetSchoolByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<schoolDTO>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var school = await clsSchools.GetByIDAsync(ID);

            if (school == null)
                return NotFound($"No School With ID {ID}, Is Not Found!");


            return Ok(school.schoolDTO);
        }

        [HttpGet("GetSchoolNameID/{SchoolName}", Name = "GetSchoolNameID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<schoolDTO>>> GetByNameAsync(string SchoolName)
        {
            if (string.IsNullOrEmpty(SchoolName))
                return BadRequest($"Invalid Data !");

            var school = await clsSchools.GetByNameAsync(SchoolName);

            if (school == null)
                return NotFound($"No School With Name {SchoolName}, Is Not Found!");


            return Ok(school.schoolDTO);
        }
    }
}
