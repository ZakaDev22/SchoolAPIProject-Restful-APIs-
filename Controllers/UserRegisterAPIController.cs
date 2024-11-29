using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/UserRegisterAPI")]
    [ApiController]
    public class UserRegisterAPIController : ControllerBase
    {
        // Complete Register Data Access And Logic Layer 
        // Alter The Get Methods In All The Logic Layer Classes With Short Hand If Logic For Clear And simple Readable Code

        [HttpGet("GetAllRegisters", Name = "GetAllRegisters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<userRegisterDTO>>> GetAllRegistersAsync()
        {
            var regesters = await clsUserRegister.GetAllAsync();

            if (regesters.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(regesters);
        }

        [HttpGet("GetRegisterByID/{ID}", Name = "GetRegisterByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<userRegisterDTO>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var register = await clsUserRegister.GetByIDAsync(ID);

            if (register == null)
                return NotFound($"No Subject With ID {ID} Is Not Found!");


            return Ok(register.RegisterDTO);
        }

    }
}
