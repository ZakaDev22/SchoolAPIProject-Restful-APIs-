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
    }
}
