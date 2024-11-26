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
    }
}
