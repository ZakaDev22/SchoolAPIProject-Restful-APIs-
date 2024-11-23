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

            var IsExists = await clsStudentGradesLog.IsExistsAsync(ID);

            if (!IsExists)
                return NotFound($"No Student Grade Log With ID {ID} Has Ben  Found!");


            return Ok(IsExists);
        }

    }
}
