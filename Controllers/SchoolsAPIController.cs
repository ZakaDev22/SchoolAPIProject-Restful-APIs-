using Microsoft.AspNetCore.Mvc;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/SchoolsAPI")]
    [ApiController]
    public class SchoolsAPIController : ControllerBase
    {
        [HttpGet("GetAllSchools", Name = "GetAllSchools")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<schoolDTO>>> GetAllSchools()
        {
            var schools = await clsSchools.GetAllAsync();

            if (schools == null)
                return NotFound("There Is No Data!");

            return Ok(schools);
        }

        [HttpGet("GetSchoolByID/{ID}", Name = "GetSchoolByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<schoolDTO>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var school = await clsSchools.GetByIDAsync(ID);

            if (school == null)
                return NotFound($"No School With ID {ID}, Is Not Found!");


            return Ok(school.schoolDTO);
        }

        [HttpGet("GetSchoolNameID/{SchoolName}", Name = "GetSchoolNameID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<schoolDTO>>> GetByNameAsync(string SchoolName)
        {
            if (string.IsNullOrEmpty(SchoolName))
                return BadRequest($"Invalid Data !");

            var school = await clsSchools.GetByNameAsync(SchoolName);

            if (school == null)
                return NotFound($"No School With Name {SchoolName}, Is Not Found!");


            return Ok(school.schoolDTO);
        }

        [HttpDelete("DeleteSchoolByID/{schoolID}", Name = "DeleteSchoolByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteSchoolByIDAsync(int schoolID)
        {
            if (schoolID <= 0)
                return BadRequest($"Invalid ID !");

            var IsExists = await clsSchools.GetByIDAsync(schoolID);

            if (IsExists is null)
                return NotFound($"No School With ID {schoolID} Has Ben  Found!");


            if (await clsSchools.DeleteAsync(schoolID))
                return Ok($"Success, School With ID {schoolID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewSchool", Name = "AddNewSchool")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<schoolDTO>> AddNewSchoolAsync(schoolDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.AddressID <= 0 || string.IsNullOrEmpty(sDTO.SchoolName))
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var school = new clsSchools(sDTO, clsSchools.enMode.AddNew);

            if (await school.SaveAsync())
            {
                return CreatedAtRoute("GetSchoolByID", new { ID = school.SchoolID }, school.schoolDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Staff Was Not Save." });
            }

        }

        [HttpPut("UpdateSchool/{ID}", Name = "UpdateSchool")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> UpdateSchoolAsync(int ID, schoolDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (ID <= 0 || sDTO.AddressID <= 0 || string.IsNullOrEmpty(sDTO.SchoolName))
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var school = await clsSchools.GetByIDAsync(ID);

            if (school == null)
                return NotFound($"No school With {ID} Have Ben Found");

            school.SchoolName = sDTO.SchoolName;
            school.AddressID = sDTO.AddressID;


            if (await school.SaveAsync())
            {
                return Ok($"Success, School With ID {school.SchoolID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, School Was Not Save." });
            }

        }
    }
}
