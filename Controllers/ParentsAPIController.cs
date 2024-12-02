using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/ParentsAPI")]
    [ApiController]
    public class ParentsAPIController : ControllerBase
    {
        [HttpGet("GetAllParents", Name = "GetAllParents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<parentDTO>>> GetAllParents()
        {
            var parents = await clsParents.GetAllAsync();

            if (parents.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(parents);
        }

        [HttpGet("GetParentByID/{ID}", Name = "GetParentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<parentDTO>>> GetByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var Parent = await clsParents.GetByIDAsync(ID);

            if (Parent == null)
                return NotFound($"No Parent With ID {ID} Is Not Found!");


            return Ok(Parent.parentDTO);
        }

        [HttpGet("IsParentExistsByID/{ID}", Name = "IsParentExistsByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsParentExistsByIDAsync(int ParentID)
        {
            if (ParentID <= 0)
                return BadRequest($"Invalid ID !");

            if (!await clsParents.IsExistsAsync(ParentID))
                return NotFound($"No Parent With ID {ParentID} Has Ben  Found!");


            return Ok(true);
        }


        [HttpGet("IsParentExistsByPersonID/{ID}", Name = "IsParentExistsByPersonID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<bool>>> IsParentExistsByPersonIDAsync(int PersonID)
        {
            if (PersonID <= 0)
                return BadRequest($"Invalid ID !");


            if (!await clsParents.IsExistsByPersonIDAsync(PersonID))
                return NotFound($"No Parent With Person ID {PersonID} Has Ben  Found!");


            return Ok(true);
        }

        [HttpDelete("DeleteParentByID/{ID}", Name = "DeleteParentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteParentByIDAsync(int ParentID)
        {
            if (ParentID <= 0)
                return BadRequest($"Invalid ID !");

            var IsExists = await clsParents.IsExistsAsync(ParentID);

            if (!IsExists)
                return NotFound($"No Student With ID {ParentID} Has Ben  Found!");


            if (await clsParents.DeleteAsync(ParentID))
                return Ok($"Success, Student With ID {ParentID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewParent", Name = "AddNewParent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<parentDTO>> AddNewParentAsync(parentDTO pDTO)
        {
            if (pDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (pDTO.PersonID <= 0 || pDTO.RelationshipTypeID < 0 || pDTO.StudentID <= 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var parent = new clsParents(pDTO, clsParents.enMode.AddNew);

            if (await parent.SaveAsync())
            {
                return CreatedAtRoute("GetParentByID", new { ID = parent.ID }, parent.parentDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Staff Was Not Save." });
            }

        }

        [HttpPut("UpdateParent/{ID}", Name = "UpdateParent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<parentDTO>> UpdateParentAsync(int ID, parentDTO pDTO)
        {
            if (pDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            // Her I Remove pDTO.ID From The Validation Because I dont Want The User To Change Persons ID Once They Are Created
            if (ID <= 0 || pDTO.RelationshipTypeID < 0 || pDTO.StudentID <= 0)
            {
                return BadRequest(" Some DTO Properties Are Empty!");
            }

            var parent = await clsParents.GetByIDAsync(ID);

            if (parent == null)
                return NotFound($"No Parent With {ID} Have Ben Found");

            //parent.ID = pDTO.ID;
            parent.RelationshipID = pDTO.RelationshipTypeID;
            parent.StudentID = pDTO.StudentID;

            if (await parent.SaveAsync())
            {
                return Ok($"Success, Student With ID {parent.ID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Staff Was Not Save." });
            }

        }
    }
}
