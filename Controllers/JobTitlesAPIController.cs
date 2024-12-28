using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/JobTitlesAPI")]
    [ApiController]
    public class JobTitlesAPIController : ControllerBase
    {
        [HttpGet("GetAllJobTitels", Name = "GetAllJobTitels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<JobTitleDTO>>> GetAllDepartmentsAsync()
        {
            var jobTitle = await clsJobTitles.GetAllAsync();

            if (jobTitle.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(jobTitle);
        }

        [HttpGet("GetJobTitleByID/{ID}", Name = "GetJobTitleByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<JobTitleDTO>>> GetDepartmentByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var jobTitle = await clsJobTitles.GetByIDAsync(ID);

            if (jobTitle == null)
                return NotFound($"No job Title With ID {ID} Is Not Found!");


            return Ok(jobTitle.jobTitleDTO);
        }

        [HttpGet("GetJobTitleByName/{title}", Name = "GetJobTitleByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<JobTitleDTO>>> GetJobTitleByNameAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
                return BadRequest($"Invalid data !");

            var jobTitle = await clsJobTitles.GetByjoobTitleNameAsync(title);

            if (jobTitle == null)
                return NotFound($"No job Title With Title : {title} Is Not Found!");


            return Ok(jobTitle.jobTitleDTO);
        }
    }
}
