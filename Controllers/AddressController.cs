using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for addresses for one addressinformation.
    /// </summary>
    [Route("api/addressinformation/{aid}/addresses")]
    [ApiController]
    public class AddressController : ControllerBase {
        private Context context;
        public AddressController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all addresses.
        /// </summary>
        /// <param name="aid">AdressInformationId</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Address[]> GetAllAddresses([FromRoute] int aid) {
            return Ok(context.Addresses.Where(v => v.AdderssInformationId == aid).ToArray());
        }

        /// <summary>
        /// Returns the address with a given id.
        /// </summary>
        /// <param name="aid">AdressInformationId</param>
        /// <param name="id">AddressId</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Address> GetAddress([FromRoute] int aid, [FromRoute] int id) {
            var value = context.Addresses.Where(v => (v.AdderssInformationId == aid) && (v.Id == id)).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds an address.
        /// </summary>
        /// <param name="aid">AddressInformationId</param>
        /// <param name="value">new Address</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Address>> AddAddress([FromRoute] int aid, [FromBody] Address value) {
            if (ModelState.IsValid) {
                //test if address already exists
                if (context.Addresses.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "Address already exists");
                    return Conflict(ModelState); //address with id already exists, we return a conflict
                }

                //test if referenced address information exists
                if (context.AddressInformation.Any(a => a.Id == aid) is false)
                {
                    ModelState.AddModelError("validationError", "AddressInformation not found");
                    return NotFound(ModelState);
                }

                value.AdderssInformationId = aid; // set reference
                context.Addresses.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the address
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Address
        }
    }
}