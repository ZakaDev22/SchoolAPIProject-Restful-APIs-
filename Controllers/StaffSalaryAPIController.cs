using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/StaffSalaryAPI")]
    [ApiController]
    public class StaffSalaryAPIController : ControllerBase
    {
        [HttpGet("GetAllStaffSalaries", Name = "GetAllStaffSalaries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<staffSalaryDTO>>> GetAllStaffSalariesAsync()
        {
            var staffSalaries = await clsStaffSalary.GetAllAsync();

            if (staffSalaries.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(staffSalaries);
        }

        [HttpGet("GetStaffSalaryByID/{ID}", Name = "GetStaffSalaryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<staffSalaryDTO>>> GetStaffSalaryByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var staffSalary = await clsStaffSalary.GetByIDAsync(ID);

            if (staffSalary == null)
                return NotFound($"No Staff Salary With ID {ID} Is Not Found!");


            return Ok(staffSalary.staffSalaryDTO);
        }

        [HttpGet("IsStaffSalaryExistsByID/{StaffSalaryID}", Name = "IsStaffSalaryExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsStaffSalaryExistsByIDAsync(int StaffSalaryID)
        {
            if (StaffSalaryID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsStaffSalary.IsExistsAsync(StaffSalaryID))
                return NotFound($"No Staff Salary With ID {StaffSalaryID} Has Ben  Found!");


            return Ok(true);
        }
    }
}
