using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/SubjectsAPI")]
    [ApiController]
    public class SubjectsAPIController : ControllerBase
    {
        [HttpGet("GetAllSubjects", Name = "GetAllSubjects")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<subjectDTO>>> GetAllSubjectsAsync()
        {
            var subject = await clsSubjects.GetAllAsync();

            if (subject.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(subject);
        }

        [HttpGet("GetSubjectByID/{ID}", Name = "GetSubjectByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<subjectDTO>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var subject = await clsSubjects.GetByIDAsync(ID);

            if (subject == null)
                return NotFound($"No Subject With ID {ID} Is Not Found!");


            return Ok(subject.sbjDTO);
        }

        [HttpGet("GetSubjectBySubjectCOde/{subjectCode}", Name = "GetSubjectBySubjectCOde")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<subjectDTO>>> GetSubjectBySubjectCOdeAsync(string subjectCode)
        {
            if (string.IsNullOrEmpty(subjectCode))
                return BadRequest($"Invalid Data !");

            var subject = await clsSubjects.GetBySubjectCodeAsync(subjectCode);

            if (subject == null)
                return NotFound($"No Subject With Code {subjectCode} Is Not Found!");


            return Ok(subject.sbjDTO);
        }
    }
}
