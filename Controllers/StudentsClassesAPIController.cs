using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/StudentsClassesAPI")]
    [ApiController]
    public class StudentsClassesAPIController : ControllerBase
    {
        [HttpGet("GetAllStudentsClasses", Name = "GetAllStudentsClasses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<studentClassDTO>>> GetAllStudentsClassesAsync()
        {
            var studentsClasses = await clsStudentsClasses.GetAllAsync();

            if (studentsClasses.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(studentsClasses);
        }

        [HttpGet("GetStudentClassByID/{ID}", Name = "GetStudentClassByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<studentClassDTO>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var studentClass = await clsStudentsClasses.GetByIDAsync(ID);

            if (studentClass == null)
                return NotFound($"No Student Class With ID {ID} Is Not Found!");


            return Ok(studentClass.sClassDTO);
        }
    }
}
