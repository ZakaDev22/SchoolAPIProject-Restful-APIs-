using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/RelationshipTypesAPI")]
    [ApiController]
    public class RelationshipTypesAPIController : ControllerBase
    {
        [HttpGet("GetAllRelationshipTypes", Name = "GetAllRelationshipTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RelationshipTypeDTO>>> GetAllRelationshipTypesAsync()
        {
            var relationships = await clsRelationshipTypes.GetAllAsync();

            if (relationships.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(relationships);
        }

        [HttpGet("GetRelationshipTypeByID/{ID}", Name = "GetRelationshipTypeByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RelationshipTypeDTO>>> GetRelationshipTypeByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var relationship = await clsRelationshipTypes.GetByIDAsync(ID);

            if (relationship == null)
                return NotFound($"No relationship With ID {ID} Is Not Found!");


            return Ok(relationship.relationshipTypeDTO);
        }
    }
}
