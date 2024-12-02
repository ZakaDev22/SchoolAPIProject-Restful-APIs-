using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/ClassesAPI")]
    [ApiController]
    public class ClassesAPIController : ControllerBase
    {
        [HttpGet("GetAllClasses", Name = "GetAllClasses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<classDTO>>> GetAllClassesAsync()
        {
            var classes = await clsClasses.GetAllAsync();

            if (classes.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(classes);
        }

        [HttpGet("GetClassByID/{ID}", Name = "GetClassByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<classDTO>>> GetClassByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var @class = await clsClasses.GetByIDAsync(ID);

            if (@class == null)
                return NotFound($"No class With ID {ID} Is Not Found!");


            return Ok(@class.classDTO);
        }

        [HttpGet("IsClassExistsByID/{ID}", Name = "IsClassExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsClassExistsByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsClasses.IsExistsAsync(ID))
                return NotFound($"No class With ID {ID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpDelete("DeleteClassByID/{ID}", Name = "DeleteClassByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteClassByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");


            if (!await clsClasses.IsExistsAsync(ID))
                return NotFound($"No CLass With ID {ID} Has Ben  Found!");


            if (await clsAttendance.DeleteAsync(ID))
                return Ok($"Success, Class With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewCLasse", Name = "AddNewCLasse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<classDTO>> AddNewCLasseAsync(classDTO classDTO)
        {
            if (classDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (classDTO.ID < 0 || classDTO.SchoolID <= 0 || classDTO.Capacity <= 0 ||
                   string.IsNullOrEmpty(classDTO.Name) || string.IsNullOrEmpty(classDTO.Code))
            {
                return base.BadRequest(" Some DTO Properties Are Empty!");
            }

            var @class = new clsClasses(classDTO, clsClasses.enMode.AddNew);

            if (await @class.SaveAsync())
            {
                return CreatedAtRoute("GetAttendanceByID", new { ID = @class.ID }, @class.classDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Attendance Was Not Save." });
            }

        }

        [HttpPut("UpdateCLass/{ID}", Name = "UpdateCLass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<classDTO>> UpdateAttendanceAsync(int ID, classDTO @class)
        {
            if (@class == null)
            {
                return BadRequest("DTO Is Null!");
            }


            if (@class.ID < 0 || @class.SchoolID <= 0 || @class.Capacity <= 0 ||
                   string.IsNullOrEmpty(@class.Name) || string.IsNullOrEmpty(@class.Code))
            {
                return base.BadRequest(" Some DTO Properties Are Empty!");
            }

            var Class = await clsClasses.GetByIDAsync(ID);

            if (Class == null)
                return NotFound($"No class With {ID} Have Ben Found");

            Class.Name = @class.Name;
            Class.Code = @class.Code;
            Class.SchoolID = @class.SchoolID;
            Class.Capacity = @class.Capacity;

            if (await Class.SaveAsync())
            {
                return Ok($"Success, Class With ID {Class.ID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Class Was Not Save." });
            }

        }
    }
}
