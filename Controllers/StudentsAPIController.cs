using Microsoft.AspNetCore.Mvc;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/StudentsAPI")]
    [ApiController]
    public class StudentsAPIController : ControllerBase
    {

        [HttpGet("GetAllStudents", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<studentDTO>>> GetAllStudents()
        {
            var students = await clsStudents.GetAllAsync();

            if (students == null)
                return NotFound("There Is No Data!");

            return Ok(students);
        }

        [HttpGet("GetByIDAsync/{ID}", Name = "GetByIDAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<studentDTO>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var student = await clsStudents.GetByIDAsync(ID);

            if (student == null)
                return NotFound($"No User With ID {ID} Is Not Found!");


            return Ok(student.sDTO);
        }

        [HttpGet("FindStudentByPersonID/{PersonID}", Name = "FindStudentByPersonID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FullUserDTO>>> FindStudentByPersonIDAsync(int PersonID)
        {
            if (PersonID <= 0)
                return BadRequest($"Invalid ID !");

            var student = await clsStudents.GetByPersonIDAsync(PersonID);

            if (student == null)
                return NotFound($"No Student With Person ID {PersonID} Has Ben  Found!");


            return Ok(student.sDTO);
        }


        [HttpGet("IsStudentExistsByID/{StudentID}", Name = "IsStudentExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsStudentExistsByIDAsync(int StudentID)
        {
            if (StudentID <= 0)
                return BadRequest($"Invalid ID !");

            var IsExists = await clsStudents.IsExistsAsync(StudentID);

            if (!IsExists)
                return NotFound($"No Student With ID {StudentID} Has Ben  Found!");


            return Ok(IsExists);
        }

        [HttpDelete("DeleteStudentByID/{StudentID}", Name = "DeleteStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteStudentByIDAsync(int StudentID)
        {
            if (StudentID <= 0)
                return BadRequest($"Invalid ID !");

            var IsExists = await clsStudents.IsExistsAsync(StudentID);

            if (!IsExists)
                return NotFound($"No Student With ID {StudentID} Has Ben  Found!");


            if (await clsStudents.DeleteAsync(StudentID))
                return Ok($"Success, Student With ID {StudentID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }
}
