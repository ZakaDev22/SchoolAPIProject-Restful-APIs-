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
                return NotFound($"No Register With ID {ID} Is Not Found!");


            return Ok(register.RegisterDTO);
        }


        [HttpDelete("DeleteRegisterByID/{ID}", Name = "DeleteRegisterByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteRegisterByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var IsExists = await clsUserRegister.GetByIDAsync(ID);

            if (IsExists is null)
                return NotFound($"No Register With ID {ID} Has Ben  Found!");


            if (await clsUserRegister.DeleteAsync(ID))
                return Ok($"Success, Register With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewRegister", Name = "AddNewRegister")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<subjectDTO>> AddNewRegisterAsync(userRegisterDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.UserID <= 0 || string.IsNullOrEmpty(sDTO.IPAddress))
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var register = new clsUserRegister(sDTO, clsUserRegister.enMode.AddNew);

            if (await register.SaveAsync())
            {
                return CreatedAtRoute("GetRegisterByID", new { ID = register.RegisterID }, register.RegisterDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Register Was Not Save." });
            }

        }

        [HttpPut("UpdateRegister/{ID}", Name = "UpdateRegister")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<userRegisterDTO>> UpdateRegisterAsync(int ID)
        {
            if (ID <= 0)
            {
                return BadRequest("Invalid ID!");
            }

            var register = await clsUserRegister.GetByIDAsync(ID);

            if (register is null)
                return NotFound($"No register With {ID} Have Ben Found");


            // if We Need To Update Register We Only Need To Call The Save Method After We Found The Record We Need 
            // Because After We Found The Record The Mode Was Set To Update Mode
            // And At The End We Only Need To Send The Record ID To The Update Method 
            // The Stored Procedure Will take care of Updating The Logout Field To The Time The Current User Is Logout
            if (await register.SaveAsync())
            {
                return Ok($"Success, Register With ID {register.RegisterID} Has Ben Updated Successfully.");
            }

            else
            {
                return StatusCode(500, new { Message = "Error, Subject Was Not Save." });
            }

        }
    }
}
