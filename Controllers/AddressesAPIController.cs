using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPiDataAccessLayer;
using SchoolBusinessLayer;

namespace SchoolWebAPIApp.Controllers
{
    [Route("api/AddressesAPI")]
    [ApiController]
    public class AddressesAPIController : ControllerBase
    {
        [HttpGet("GetAllAddresses", Name = "GetAllAddresses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<addressDTO>>> GetAllAddressesAsync()
        {
            var addresses = await clsAddresses.GetAllAsync();

            if (addresses.IsNullOrEmpty())
                return NotFound("There Is No Data!");

            return Ok(addresses);
        }

        [HttpGet("GetAddressByID/{ID}", Name = "GetAddressByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<attendanceDTO>>> GetAddressByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");

            var address = await clsAddresses.GetByIDAsync(ID);

            if (address == null)
                return NotFound($"No address With ID {ID} Is Not Found!");


            return Ok(address.addressDTO);
        }

        [HttpGet("GetAddressByCity/{city}", Name = "GetAddressByCity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<addressDTO>>> GetAddressByCityAsync(string city)
        {
            if (string.IsNullOrEmpty(city))
                return BadRequest($"Invalid ID !");

            var address = await clsAddresses.GetByCityNameAsync(city);

            if (address == null)
                return NotFound($"No addresses With Name CountryID {city} Is Not Found!");


            return Ok(address);
        }

        [HttpDelete("DeleteAddressByID/{ID}", Name = "DeleteAddressByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<bool>>> DeleteAddressByIDAsync(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Invalid ID !");


            var address = await clsAddresses.GetByIDAsync(ID);

            if (address is null)
                return NotFound($"No Address With ID {ID} Has Ben  Found!");


            if (await clsAddresses.DeleteAsync(ID))
                return Ok($"Success, Address With ID {ID} Has Ben Deleted.");
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("AddNewAddress", Name = "AddNewAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<attendanceDTO>> AddNewAddressAsync(addressDTO addDTO)
        {
            if (addDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (addDTO.StateID < 0 || addDTO.CountryID <= 0 || string.IsNullOrEmpty(addDTO.Street) || string.IsNullOrEmpty(addDTO.City))
            {
                return base.BadRequest(" Some DTO Properties Are Empty!");
            }

            var address = new clsAddresses(addDTO, clsAddresses.enMode.AddNew);

            if (await address.SaveAsync())
            {
                return CreatedAtRoute("GetAddressByID", new { ID = address.ID }, address.addressDTO);
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Address Was Not Save." });
            }

        }

        [HttpPut("UpdateAddress/{ID}", Name = "UpdateAddress")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<attendanceDTO>> UpdateAddressAsync(int ID, addressDTO addDTO)
        {
            if (addDTO == null)
            {
                return BadRequest("DTO Is Null!");
            }

            if (ID <= 0 || addDTO.StateID < 0 || addDTO.CountryID <= 0 || string.IsNullOrEmpty(addDTO.Street) || string.IsNullOrEmpty(addDTO.City))
            {
                return base.BadRequest(" Some DTO Properties Are Empty!");
            }

            var address = await clsAddresses.GetByIDAsync(ID);

            if (address == null)
                return NotFound($"No address With {ID} Have Ben Found");

            //address.ID = addDTO.ID;
            address.Street = addDTO.Street;
            address.StateID = addDTO.StateID;
            address.City = addDTO.City;
            address.CountryID = addDTO.CountryID;

            if (await address.SaveAsync())
            {
                return Ok($"Success, Address With ID {address.ID} Has Ben Updated Successfully.");
            }
            else
            {
                return StatusCode(500, new { Message = "Error, Address Was Not Save." });
            }

        }

    }
}
