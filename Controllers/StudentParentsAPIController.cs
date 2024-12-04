using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/StudentParentsAPI")]
    [ApiController]
    public class StudentParentsAPIController : ControllerBase
    {
        [HttpGet("GetAllStudentsParents", Name = "GetAllStudentsParents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<studentClassDTO>>> GetAllStudentsParentsAsync()
        {
            var studentsParents = await clsStudentParents.GetAllAsync();

            if (studentsParents.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(studentsParents);
        }

        [HttpGet("GetStudentParentByID/{ID}", Name = "GetStudentParentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<studentParentDTO>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var studentParent = await clsStudentParents.GetByIDAsync(ID);

            if (studentParent == null)
                return NotFound($"No Student Parent With ID {ID}, Is Not Found!");


            return Ok(studentParent.sParentDTO);
        }

        [HttpGet("IsStudentParentExistsByID/{ID}", Name = "IsStudentParentExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsStudentParentExistsByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsStudentParents.IsExistsAsync(ID))
                return NotFound($"No Student Parent With ID {ID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpDelete("DeleteStudentParentByID/{ID}", Name = "DeleteStudentParentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteStudentParentByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsStudentParents.IsExistsAsync(ID))
                return NotFound($"No Student Parent With ID {ID} Has Ben  Found!");


            if (await clsStudentParents.DeleteAsync(ID))
                return Ok($"Success, Student Parent With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewStudentParent", Name = "AddNewStudentParent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<studentClassDTO>> AddNewStudentParentAsync(studentParentDTO sDTO)
        {
            if (sDTO is null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.StudentID <= 0 || sDTO.ParentID < 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var studentParent = new clsStudentParents(sDTO, clsStudentParents.enMode.AddNew);

            if (await studentParent.SaveAsync())
            {
                return CreatedAtRoute("GetStudentParentByID", new { ID = studentParent.ID }, studentParent.sParentDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Student Parent Was Not Save." });
            }

        }

        [HttpPut("UpdateStudentParent/{ID}", Name = "UpdateStudentParent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<studentParentDTO>> UpdateStudentParentAsync(int ID, studentParentDTO sDTO)
        {
            if (sDTO is null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.StudentID <= 0 || sDTO.ParentID < 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var studentParent = await clsStudentParents.GetByIDAsync(ID);

            if (studentParent is null)
                return NotFound($"No Student Parent With {ID} Have Ben Found");

            studentParent.StudentID = sDTO.StudentID;
            studentParent.ParentID = sDTO.ParentID;

            if (await studentParent.SaveAsync())
            {
                return Ok($"Success, Student Parent With ID {studentParent.ID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Student Parent Was Not Save." });
            }

        }
    }
}
