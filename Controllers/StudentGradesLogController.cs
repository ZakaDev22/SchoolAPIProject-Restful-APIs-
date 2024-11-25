using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/StudentGradesLog")]
    [ApiController]
    public class StudentGradesLogController : ControllerBase
    {
        [HttpGet("GetAllStudentsGradeLogs", Name = "GetAllStudentsGradeLogs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<sgLogDTO>>> GetAllStudentsGradeLogsAsync()
        {
            var sglogs = await clsStudentGradesLog.GetAllAsync();

            if (sglogs.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(sglogs);
        }

        [HttpGet("GetStudentGradeLogByID/{ID}", Name = "GetStudentGradeLogByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<sgLogDTO>>> GetStudentGradeLogByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var sglog = await clsStudentGradesLog.GetByIDAsync(ID);

            if (sglog == null)
                return NotFound($"No Student Grade Log With ID {ID} Is Not Found!");


            return Ok(sglog.sgLogDTO);
        }


        [HttpGet("IsStudentGradeLogExistsByID/{ID}", Name = "IsStudentGradeLogExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsStudentGradeLogExistsByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");


            if (!await clsStudentGradesLog.IsExistsAsync(ID))
                return NotFound($"No Student Grade Log With ID {ID} Has Ben  Found!");

            return Ok(true);
        }


        [HttpDelete("DeleteStudentGradeLogByID/{ID}", Name = "DeleteStudentGradeLogByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteStudentGradeLogByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var IsExists = await clsStudentGradesLog.GetByIDAsync(ID);

            if (IsExists is null)
                return NotFound($"No Student Grade Log With ID {ID} Has Ben  Found!");


            if (await clsStudentGradesLog.DeleteAsync(ID))
                return Ok($"Success, Student Grade Log With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewStudentGradeLog", Name = "AddNewStudentGradeLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<sgLogDTO>> AddNewStudentGradeLogAsync(sgLogDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.StudentID <= 0 || sDTO.SubjectID <= 0 || sDTO.Grade < 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var sgLog = new clsStudentGradesLog(sDTO, clsStudentGradesLog.enMode.AddNew);

            if (await sgLog.SaveAsync())
            {
                return CreatedAtRoute("GetStudentGradeLogByID", new { ID = sgLog.LogID }, sgLog.sgLogDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Subject Was Not Save." });
            }

        }

        [HttpPut("UpdateStudentGradeLog/{ID}", Name = "UpdateStudentGradeLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<sgLogDTO>> UpdateStudentGradeLogAsync(int ID, sgLogDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.StudentID <= 0 || sDTO.SubjectID <= 0 || sDTO.Grade < 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var sgLog = await clsStudentGradesLog.GetByIDAsync(ID);

            if (sgLog is null)
                return NotFound($"No sgLog With {ID} Have Ben Found");

            sgLog.StudentID = sDTO.StudentID;
            sgLog.SubjectID = sDTO.SubjectID;
            sgLog.Grade = sDTO.Grade;
            sgLog.Comments = sDTO.Comments ?? string.Empty;


            if (await sgLog.SaveAsync())
            {
                return Ok($"Success,  Grade Log With ID {sgLog.LogID} Has Ben Updated Successfully.");
            }

            else
            {
                return StatusCode(500, new { Message = "Error, Grade Log Was Not Save." });
            }

        }
    }
}
