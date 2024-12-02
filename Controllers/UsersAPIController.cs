using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

            if (users.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(users);
        }

        [HttpGet("GetUserByID/{UserID}", Name = "GetUserByID")]
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

        [HttpDelete("DeleteUserAsync/{UserID}", Name = "DeleteUserAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteUserAsync(int UserID)
        {
            if (UserID <= 0)
                return BadRequest($"Invalid User ID !");

            var user = await clsUsers.GetUserByIdAsync(UserID);

            if (user == null)
                return NotFound($"No User With ID {UserID} Is Not Found!");

            if (await clsUsers.DeleteUserAsync(UserID))
            {
                return Ok($"User With ID {UserID} Was Successfully Deleted.");
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error, Something Went Wrong In the Server, Please Try Again");
        }

        [HttpGet("IsUserExistAsync/{UserID}", Name = "IsUserExistAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsUserExistAsync(int UserID)
        {
            if (UserID <= 0)
                return BadRequest($"Invalid User ID !");

            var user = await clsUsers.GetUserByIdAsync(UserID);

            if (user == null)
                return NotFound($"{false}, No User With ID {UserID}, User Is Not Found!");


            return Ok(true);
        }

        [HttpPost("AddNewUser", Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FullUserDTO>> AddNewUserAsync(FullUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("user DTO Is Null!");
            }

            if (string.IsNullOrEmpty(userDTO.UserName) || userDTO.PersonID <= 0 || userDTO.Permissions < 0
                                                       || userDTO.AddedByUserID <= 0 || string.IsNullOrEmpty(userDTO.Password))
            {
                return BadRequest(" Some User DTO Properties Are Empty!");
            }

            clsUsers user = new clsUsers(clsUsers.enMode.AddNew, userDTO);

            if (await user.SaveAsync())
            {
                return CreatedAtRoute("GetUserByID", new { UserID = user.ID }, user.UserDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Person Was Not Save." });
            }

        }

        [HttpPut("UpdateUser/{ID}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FullUserDTO>> UpdateUser(int ID, FullUserDTO userDTO)
        {
            if (userDTO == null || ID <= 0)
            {
                return BadRequest("Invalid ID Or user DTO Is Null!");
            }

            if (userDTO.PersonID <= 0 || userDTO.Permissions < 0 || string.IsNullOrEmpty(userDTO.Password) || string.IsNullOrEmpty(userDTO.UserName))
            {
                return BadRequest(" Some User DTO Properties Are Empty!");
            }

            clsUsers user = await clsUsers.GetUserByIdAsync(ID);

            if (user is null)
            {
                return NotFound($"No User With ID {ID} Have Ben Found!");
            }

            user.personID = userDTO.PersonID;
            user.permissions = userDTO.Permissions;
            user.passwordHash = userDTO.Password;
            user.userName = userDTO.UserName;

            if (await user.SaveAsync())
            {
                return Ok($"User With ID {ID} has Ben Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Person Was Not Save." });
            }

        }
    }
}
