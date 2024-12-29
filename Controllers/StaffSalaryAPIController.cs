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

        [HttpGet("IsStaffSalaryExistsByID/{ID}", Name = "IsStaffSalaryExistsByID")]
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

        [HttpDelete("DeleteStaffSalaryByID/{ID}", Name = "DeleteStaffSalaryByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteStaffSalaryByIDAsync(int StaffSalaryID)
        {
            if (StaffSalaryID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsStaffSalary.IsExistsAsync(StaffSalaryID))
                return NotFound($"No Staff Salary With ID {StaffSalaryID} Has Ben  Found!");


            if (await clsStaffSalary.DeleteAsync(StaffSalaryID))
                return Ok($"Success, Staff With ID {StaffSalaryID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewStaffSalary", Name = "AddNewStaffSalary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<staffSalaryDTO>> AddNewStaffSalaryAsync(staffSalaryDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.StaffID <= 0 || sDTO.Salary < 0 || sDTO.Bonus <= 0 || sDTO.Deductions < 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var staffSalary = new clsStaffSalary(sDTO, clsStaffSalary.enMode.AddNew);

            if (await staffSalary.SaveAsync())
            {
                return CreatedAtRoute("GetStaffSalaryByID", new { ID = staffSalary.ID }, staffSalary.staffSalaryDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Staff Salary Was Not Save." });
            }

        }

        [HttpPut("UpdateStaffSalary/{ID}", Name = "UpdateStaffSalary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<staffSalaryDTO>> UpdateStaffSalaryAsync(int ID, staffSalaryDTO sDTO)
        {
            if (sDTO is null)
            {
                return BadRequest("DTO Is Null!");
            }

            // in This Case We Dont Need To Check If ID Or The Effective StateID Are valid Because We Will Not Update Theme Anyway
            if (sDTO.Salary < 0 || sDTO.Bonus <= 0 || sDTO.Deductions < 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var staffSalary = await clsStaffSalary.GetByIDAsync(ID);

            if (staffSalary == null)
                return NotFound($"No staffSalary With {ID} Have Ben Found");

            // in This Case We Dont Need To Update The ID Or The Effective StateID 
            staffSalary.Salary = sDTO.Salary;
            staffSalary.Bonus = sDTO.Bonus;
            staffSalary.Deductions = sDTO.Deductions;

            if (await staffSalary.SaveAsync())
            {
                return Ok($"Success, Staff Salary With ID {staffSalary.StaffID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Staff Salary Was Not Save." });
            }

        }
    }
}
