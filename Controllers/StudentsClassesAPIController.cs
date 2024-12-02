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

        [HttpGet("IsStudentClassExistsByID/{ID}", Name = "IsStudentClassExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsStudentClassExistsByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsStudentsClasses.IsExistsAsync(ID))
                return NotFound($"No Student Class With ID {ID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpDelete("DeleteStudentClassByID/{ID}", Name = "DeleteStudentClassByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteStudentClassByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsStudentsClasses.IsExistsAsync(ID))
                return NotFound($"No Student Class With ID {ID} Has Ben  Found!");


            if (await clsStudentsClasses.DeleteAsync(ID))
                return Ok($"Success, Student Class With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
