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
                return NotFound($"No Subject With Name {subjectCode} Is Not Found!");


            return Ok(subject.sbjDTO);
        }

        [HttpGet("IsSubjectExistsByID/{ID}", Name = "IsSubjectExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsSubjectExistsByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsSubjects.IsExistsAsync(ID))
                return NotFound($"No subject With ID {ID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpDelete("DeleteSubjectByID/{ID}", Name = "DeleteSubjectByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteSubjectByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var IsExists = await clsSubjects.GetByIDAsync(ID);

            if (IsExists is null)
                return NotFound($"No Subject With ID {ID} Has Ben  Found!");


            if (await clsSubjects.DeleteAsync(ID))
                return Ok($"Success, Subject With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewSubject", Name = "AddNewSubject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<subjectDTO>> AddNewSubjectAsync(subjectDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.SchoolID <= 0 || string.IsNullOrEmpty(sDTO.Name) || string.IsNullOrEmpty(sDTO.Code))
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var subject = new clsSubjects(sDTO, clsSubjects.enMode.AddNew);

            if (await subject.SaveAsync())
            {
                return CreatedAtRoute("GetSubjectByID", new { ID = subject.ID }, subject.sbjDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Subject Was Not Save." });
            }

        }

        [HttpPut("UpdateSubject/{ID}", Name = "UpdateSubject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<subjectDTO>> UpdateSubjectAsync(int ID, subjectDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.SchoolID <= 0 || string.IsNullOrEmpty(sDTO.Name) || string.IsNullOrEmpty(sDTO.Code))
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var subject = await clsSubjects.GetByIDAsync(ID);

            if (subject == null)
                return NotFound($"No subject With {ID} Have Ben Found");

            subject.Name = sDTO.Name;
            subject.Code = sDTO.Code;
            subject.SchoolID = sDTO.SchoolID;


            if (await subject.SaveAsync())
            {
                return Ok($"Success, Subject With ID {subject.ID} Has Ben Updated Successfully.");
            }

            else
            {
                return StatusCode(500, new { Message = "Error, Subject Was Not Save." });
            }

        }
    }
}
