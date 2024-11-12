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

        [HttpGet("GetUserByIDAsync/{UserID}", Name = "GetUserByIDAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FullUserDTO>>> GetUserByIDAsync(int UserID)
        {
            if (UserID <= 0)
                return BadRequest($"Invalid User ID !");

            var user = await clsUsers.GetUserByIdAsync(UserID);

            if (user == null)
                return NotFound($"No User With ID {UserID} Is Not Found!");


            return Ok(user.fullUserDTO);
        }

        [HttpGet("FindUserByUserNameAndPasswordAsync/{UserName}/{Password}", Name = "FindUserByUserNameAndPasswordAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FullUserDTO>>> FindUserByUserNameAndPasswordAsync(string UserName, string Password)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                return BadRequest($"Invalid User ID !");

            var user = await clsUsers.AuthenticateUserAsync(UserName, Password);

            if (user == null)
                return NotFound($"No User With UserName {UserName} And Pasword {Password} Is Not Found!");


            return Ok(user.fullUserDTO);
        }

    }
}
