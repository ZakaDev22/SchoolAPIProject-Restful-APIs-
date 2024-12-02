using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<ActionResult<IEnumerable<studentDTO>>> GetAllStudentsAsync()
        {
            var students = await clsStudents.GetAllAsync();

            if (students.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(students);
        }

        [HttpGet("GetStudentByID/{ID}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<studentDTO>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var student = await clsStudents.GetByIDAsync(ID);

            if (student == null)
                return NotFound($"No Student With ID {ID} Is Not Found!");


            return Ok(student.sDTO);
        }

        [HttpGet("FindStudentByPersonID/{ID}", Name = "FindStudentByPersonID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<studentDTO>>> FindStudentByPersonIDAsync(int PersonID)
        {
            if (PersonID <= 0)
                return BadRequest($"Invalid ID !");

            var student = await clsStudents.GetByPersonIDAsync(PersonID);

            if (student == null)
                return NotFound($"No Student With Person ID {PersonID} Has Ben  Found!");


            return Ok(student.sDTO);
        }


        [HttpGet("IsStudentExistsByID/{ID}", Name = "IsStudentExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsStudentExistsByIDAsync(int StudentID)
        {
            if (StudentID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsStudents.IsExistsAsync(StudentID))
                return NotFound($"No Student With ID {StudentID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpDelete("DeleteStudentByID/{ID}", Name = "DeleteStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteStudentByIDAsync(int StudentID)
        {
            if (StudentID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsStudents.IsExistsAsync(StudentID))
                return NotFound($"No Student With ID {StudentID} Has Ben  Found!");


            if (await clsStudents.DeleteAsync(StudentID))
                return Ok($"Success, Student With ID {StudentID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewStudent", Name = "AddNewStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<studentDTO>> AddNewStudentAsync(studentDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.PersonID <= 0 || sDTO.StudentGradeID < 0 || sDTO.SchoolID <= 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var student = new clsStudents(sDTO, clsStudents.enMode.AddNew);

            if (await student.SaveAsync())
            {
                return CreatedAtRoute("GetStudentByID", new { ID = student.ID }, student.sDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Person Was Not Save." });
            }

        }

        [HttpPut("UpdateStudent/{ID}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<studentDTO>> UpdateStudentAsync(int ID, studentDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (ID <= 0 || sDTO.PersonID <= 0 || sDTO.StudentGradeID < 0 || sDTO.SchoolID <= 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var student = await clsStudents.GetByIDAsync(ID);

            if (student == null)
                return NotFound($"No Student With {ID} Have Ben Found");

            student.PersonID = sDTO.PersonID;
            student.StudentGradeID = sDTO.StudentGradeID;
            student.SchoolID = sDTO.SchoolID;
            student.IsActive = sDTO.IsActive;

            if (await student.SaveAsync())
            {
                return Ok($"Success, Student With ID {student.ID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Student Was Not Save." });
            }

        }
    }
}
