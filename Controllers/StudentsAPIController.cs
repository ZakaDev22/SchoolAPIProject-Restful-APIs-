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
            var users = await clsStudents.GetAllAsync();

            if (users == null)
                return NotFound("There Is No Data!");

            return Ok(users);
        }


    }
}
