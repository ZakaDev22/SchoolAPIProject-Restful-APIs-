using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/AttendanceAPI")]
    [ApiController]
    public class AttendanceAPIController : ControllerBase
    {
        [HttpGet("GetAllAttendance", Name = "GetAllAttendance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<attendanceDTO>>> GetAllAttendanceAsync()
        {
            var Attendances = await clsAttendance.GetAllAsync();

            if (Attendances.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(Attendances);
        }

        [HttpGet("GetAttendanceByID/{ID}", Name = "GetAttendanceByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<attendanceDTO>>> GetAttendanceByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var attendance = await clsAttendance.GetByIDAsync(ID);

            if (attendance == null)
                return NotFound($"No attendance With ID {ID} Is Not Found!");


            return Ok(attendance.attendanceDTO);
        }

        [HttpGet("IsAttendanceExistsByID/{ID}", Name = "IsAttendanceExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsAttendanceExistsByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsAttendance.IsExistsAsync(ID))
                return NotFound($"No attendance With ID {ID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpDelete("DeleteAttendanceByID/{ID}", Name = "DeleteAttendanceByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteAttendanceByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");


            if (!await clsAttendance.IsExistsAsync(ID))
                return NotFound($"No Attendance With ID {ID} Has Ben  Found!");


            if (await clsAttendance.DeleteAsync(ID))
                return Ok($"Success, Attendance With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewAttendance", Name = "AddNewAttendance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<attendanceDTO>> AddNewAttendanceAsync(attendanceDTO attendanceDTO)
        {
            if (attendanceDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (attendanceDTO.ClassID < 0 || attendanceDTO.ClassID <= 0)
            {
                return base.BadRequest(" Some DTO Properties Are Empty!");
            }

            var attendance = new clsAttendance(attendanceDTO, clsAttendance.enMode.AddNew);

            if (await attendance.SaveAsync())
            {
                return CreatedAtRoute("GetAttendanceByID", new { ID = attendance.AttendanceID }, attendance.attendanceDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Attendance Was Not Save." });
            }

        }

        [HttpPut("UpdateAttendance/{ID}", Name = "UpdateAttendance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<attendanceDTO>> UpdateAttendanceAsync(int ID, attendanceDTO attendanceDTO)
        {
            if (attendanceDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (ID <= 0 || attendanceDTO.StudentID < 0 || attendanceDTO.ClassID <= 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var attendance = await clsAttendance.GetByIDAsync(ID);

            if (attendance == null)
                return NotFound($"No attendance With {ID} Have Ben Found");

            //attendance.PersonID = attendanceDTO.PersonID;
            attendance.StudentID = attendanceDTO.StudentID;
            attendance.ClassID = attendanceDTO.ClassID;
            attendance.Status = attendanceDTO.Status;

            if (await attendance.SaveAsync())
            {
                return Ok($"Success, Attendance With ID {attendance.AttendanceID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Staff Was Not Save." });
            }

        }
    }
}
