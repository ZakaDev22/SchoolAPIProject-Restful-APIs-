﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/StaffAPI")]
    [ApiController]
    public class StaffAPIController : ControllerBase
    {
        [HttpGet("GetAllStaff", Name = "GetAllStaff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<staffDTO>>> GetAllStaffAsync()
        {
            var staffs = await clsStaff.GetAllAsync();

            if (staffs.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(staffs);
        }

        [HttpGet("GetStaffByID/{ID}", Name = "GetStaffByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<staffDTO>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var staff = await clsStaff.GetByIDAsync(ID);

            if (staff == null)
                return NotFound($"No Staff With ID {ID} Is Not Found!");


            return Ok(staff.staffDTO);
        }

        [HttpGet("IsStaffExistsByID/{ID}", Name = "IsStaffExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsStaffExistsByIDAsync(int StaffID)
        {
            if (StaffID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsStaff.IsExistsAsync(StaffID))
                return NotFound($"No Staff With ID {StaffID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpGet("IsStaffExistsByPersonID/{ID}", Name = "IsStaffExistsByPersonID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsStaffExistsByPersonIDAsync(int PersonID)
        {
            if (PersonID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsStaff.IsExistsByPersonIDAsync(PersonID))
                return NotFound($"No Staff With Person ID {PersonID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpDelete("DeleteStaffByID/{ID}", Name = "DeleteStaffByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteStaffByIDAsync(int StaffID)
        {
            if (StaffID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsStaff.IsExistsAsync(StaffID))
                return NotFound($"No Staff With ID {StaffID} Has Ben  Found!");


            if (await clsStaff.DeleteAsync(StaffID))
                return Ok($"Success, Staff With ID {StaffID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewStaff", Name = "AddNewStaff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<staffDTO>> AddNewStaffAsync(staffDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.PersonID <= 0 || sDTO.JobTitleID < 0 || sDTO.SchoolID <= 0
                                    || sDTO.DepartmentID < 0 || sDTO.StaffSalaryID <= 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var staff = new clsStaff(sDTO, clsStaff.enMode.AddNew);

            if (await staff.SaveAsync())
            {
                return CreatedAtRoute("GetStaffByID", new { ID = staff.ID }, staff.staffDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Staff Was Not Save." });
            }

        }

        [HttpPut("UpdateStaff/{ID}", Name = "UpdateStaff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<staffDTO>> UpdateStaffAsync(int ID, staffDTO sDTO)
        {
            if (sDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (sDTO.PersonID <= 0 || sDTO.JobTitleID < 0 || sDTO.SchoolID <= 0
                                   || sDTO.DepartmentID < 0 || sDTO.StaffSalaryID <= 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var staff = await clsStaff.GetByIDAsync(ID);

            if (staff is null)
                return NotFound($"No staff With {ID} Have Ben Found");

            staff.PersonID = sDTO.PersonID;
            staff.JobTitleID = sDTO.JobTitleID;
            staff.SchoolID = sDTO.SchoolID;
            staff.DepartmentID = sDTO.DepartmentID;
            staff.StaffSalaryID = sDTO.StaffSalaryID;

            if (await staff.SaveAsync())
            {
                return Ok($"Success, Staff With ID {staff.ID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Staff Was Not Save." });
            }

        }
    }
}
