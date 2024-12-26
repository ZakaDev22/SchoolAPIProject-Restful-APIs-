using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/StudentGradesAPI")]
    [ApiController]
    public class StudentGradesAPIController : ControllerBase
    {
        [HttpGet("GetAllStudentsGrades", Name = "GetAllStudentsGrades")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<sGradeDTO>>> GetAllStudentsGradesAsync()
        {
            var sGrades = await clsStudentGrades.GetAllAsync();

            if (sGrades.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(sGrades);
        }

        [HttpGet("GetStudentGradeByID/{ID}", Name = "GetStudentGradeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<sGradeDTO>>> GetStudentGradeByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var sGrade = await clsStudentGrades.GetByIDAsync(ID);

            if (sGrade == null)
                return NotFound($"No Student Grade With ID {ID} Is Not Found!");


            return Ok(sGrade.sGradeDTO);
        }


        [HttpGet("IsStudentGradeExistsByID/{ID}", Name = "IsStudentGradeExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsStudentGradeExistsByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");


            if (!await clsStudentGrades.IsExistsAsync(ID))
                return NotFound($"No Student Grade With ID {ID} Has Ben  Found!");

            return Ok(true);
        }


        [HttpDelete("DeleteStudentGradeByID/{ID}", Name = "DeleteStudentGradeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteStudentGradeByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var IsExists = await clsStudentGrades.GetByIDAsync(ID);

            if (IsExists is null)
                return NotFound($"No Student Grade With ID {ID} Has Ben  Found!");


            if (await clsStudentGrades.DeleteAsync(ID))
                return Ok($"Success, Student Grade  With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
