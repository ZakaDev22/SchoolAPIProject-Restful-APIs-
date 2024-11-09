using Microsoft.AspNetCore.Mvc;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/UsersAPI")]
    [ApiController]
    public class UsersAPIController : ControllerBase
    {
        // GET: api/Users
        [HttpGet("GetAllUsers", Name = "GetAllUsersAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await clsUsers.GetAllUsersAsync();

            if (users == null)
                return NotFound("There Is No Data!");

            return Ok(users);
        }
    }
}
