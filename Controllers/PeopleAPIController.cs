using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/PeopleAPI")]
    [ApiController]
    public class PeopleAPIController : ControllerBase
    {
        [HttpGet("GetAll", Name = "GetAllPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetAll()
        {
            IEnumerable<PersonDTO> PeopleList = await clsPeople.GetAllAsync();

            if (PeopleList.IsNullOrEmpty())
            {
                return NotFound("There Is No People In The Database!");
            }

            return Ok(PeopleList);
        }

        [HttpGet("GetByID/{ID}", Name = "GetByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDTO>> GetByID(int ID)
        {
            if (ID <= 0)
            {
                return BadRequest($"Bad Request With ID = {ID}");
            }

            PersonDTO person = await clsPeople.GetByIDAsync(ID);

            if (person is null)
            {
                return NotFound($"There Is No Person With ID {ID} ");
            }

            return Ok(person);
        }

    }
}
