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
    }
}
