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
                return NotFound($"No addresses With City Name {city} Is Not Found!");


            return Ok(address);
        }

    }
}
