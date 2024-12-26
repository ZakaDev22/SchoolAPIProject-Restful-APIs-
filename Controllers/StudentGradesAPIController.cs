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
    }
}
