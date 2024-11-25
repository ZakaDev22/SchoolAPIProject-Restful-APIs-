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
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetAllAsync()
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
        public async Task<ActionResult<PersonDTO>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
            {
                return BadRequest($"Bad Request With ID = {ID}");
            }

            clsPeople person = await clsPeople.GetByIDAsync(ID);

            if (person is null)
            {
                return NotFound($"There Is No Person With ID {ID} ");
            }

            return Ok(person.pDTO);
        }

        [HttpGet("GetByFullName", Name = "GetByFullName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDTO>> GetByNameAsync(string FirstName, string LastName)
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            {
                return BadRequest($"Bad Request With Full Name FirstName Or LastName");
            }

            clsPeople person = await clsPeople.GetByNameAsync(FirstName, LastName);

            if (person is null)
            {
                return NotFound($"There Is No Person Name {FirstName} {LastName} ");
            }

            return Ok(person.pDTO);
        }

        [HttpPost("AddNewPerson", Name = "AddNewPerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDTO>> AddNewPersonAsync(PersonDTO personDTO)
        {
            if (personDTO is null)
            {
                return BadRequest("Person DTO In Null!");
            }

            if (string.IsNullOrEmpty(personDTO.FirstName) || string.IsNullOrEmpty(personDTO.LastName) || personDTO.SchoolID <= 0 || personDTO.AddressID <= 0)
            {
                return BadRequest(" Some Person DTO Properties Are Empty!");
            }

            clsPeople person = new clsPeople(personDTO, clsPeople.enMode.AddNew);

            if (await person.SaveAsync())
            {
                var newPerson = personDTO with { PersonID = person.PersonID };


                return CreatedAtRoute("GetByID", new { ID = newPerson.PersonID }, newPerson);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Person Was Not Save." });
            }

        }

        [HttpPut("UpdatePerson", Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonDTO>> UpdatePersonAsync(int ID, PersonDTO personDTO)
        {
            if (personDTO is null)
            {
                return BadRequest("Person DTO In Null!");
            }

            if (string.IsNullOrEmpty(personDTO.FirstName) || string.IsNullOrEmpty(personDTO.LastName) || personDTO.SchoolID <= 0 || personDTO.AddressID <= 0)
            {
                return BadRequest(" Some Person DTO Properties Are Empty!");
            }

            clsPeople person = await clsPeople.GetByIDAsync(ID);

            if (person is null)
            {
                NotFound("Error, Person Was Not Found!");
            }

            person.FirstName = personDTO.FirstName;
            person.LastName = personDTO.LastName;
            person.DateOfBirth = personDTO.DateOfBirth;
            person.Gender = personDTO.Gender;
            person.SchoolID = personDTO.SchoolID;
            person.AddressID = personDTO.AddressID;

            if (await person.SaveAsync())
            {
                return Ok($"Person With ID {person.PersonID} Have Ben Updated successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Person Was Not Updated." });
            }

        }

        [HttpDelete("DeletePerson", Name = "DeletePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeletePersonAsync(int ID)
        {
            if (ID <= 0)
            {
                return BadRequest($"Invalid ID : {ID} !!!");
            }

            clsPeople person = await clsPeople.GetByIDAsync(ID);

            if (person is null)
            {
                return NotFound($"Not Person Have Ben Found With ID {ID}");
            }

            if (!await clsPeople.DeleteAsync(person.PersonID))
            {
                return StatusCode(500, new { Messgae = "Error, Person Was Not Deleted" });
            }
            else
                return Ok($"Success, Person With ID {ID} Have Ben Deleted Successfully.");
        }

        [HttpGet("IsPersonExistsByID/{PersonID}", Name = "IsPersonExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsPersonExistsByIDAsync(int PersonID)
        {
            if (PersonID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsPeople.IsExistsByIDAsync(PersonID))
                return NotFound($"No Student With ID {PersonID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpGet("IsPersonExistsByName/{FirstName}/{LastName}", Name = "IsPersonExistsByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsPersonExistsByNameAsync(string FirstName, string LastName)
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
                return BadRequest($"Invalid ID !");

            var IsExists = await clsPeople.IsExistsByNameAsync(FirstName, LastName);

            if (!IsExists)
                return NotFound($"No Student With Name {FirstName} {LastName} Has Ben  Found!");


            return Ok(IsExists);
        }
    }
}
