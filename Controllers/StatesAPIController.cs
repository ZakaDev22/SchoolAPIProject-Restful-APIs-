using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/StatesAPI")]
    [ApiController]
    public class StatesAPIController : ControllerBase
    {
        [HttpGet("GetAllStates", Name = "GetAllStates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<stateDTO>>> GetAllStatesAsync()
        {
            var states = await clsStates.GetAllAsync();

            if (states.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(states);
        }

        [HttpGet("GetAllStatesByCountryID/{ID}", Name = "GetAllStatesByCountryID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<stateDTO>>> GetAllStatesByCountryIDAsync(int ID)
        {
            var states = await clsStates.GetAllStatsByCountryIDAsync(ID);

            if (states.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(states);
        }

        [HttpGet("GetStateByID/{ID}", Name = "GetStateByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<stateDTO>>> GetStateByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var state = await clsStates.GetByIDAsync(ID);

            if (state == null)
                return NotFound($"No state With ID {ID} Is Not Found!");


            return Ok(state.stateDTO);
        }

        [HttpGet("GetStateByName/{Name}", Name = "GetStateByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<stateDTO>>> GetStateByNameAsync(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return BadRequest($"Invalid Data !");

            var state = await clsStates.GetByStateNameAsync(Name);

            if (state == null)
                return NotFound($"No state With Name {Name} Is Not Found!");


            return Ok(state.stateDTO);
        }
    }
}
