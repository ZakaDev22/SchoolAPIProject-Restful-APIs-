using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/DepartmentsAPI")]
    [ApiController]
    public class DepartmentsAPIController : ControllerBase
    {
        [HttpGet("GetAllDepartments", Name = "GetAllDepartments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<departmentDTO>>> GetAllDepartmentsAsync()
        {
            var departments = await clsDepartments.GetAllAsync();

            if (departments.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(departments);
        }

        [HttpGet("GetDepartmentByID/{ID}", Name = "GetDepartmentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<departmentDTO>>> GetDepartmentByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var department = await clsDepartments.GetByIDAsync(ID);

            if (department == null)
                return NotFound($"No department With ID {ID} Is Not Found!");


            return Ok(department.departmentDTO);
        }
    }
}
