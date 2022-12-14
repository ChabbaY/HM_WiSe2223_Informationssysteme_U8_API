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
    [Route("api/suppliers/{sid}/addresses")]
    [ApiController]
    public class AddressController : ControllerBase {
        private Context context;
        public AddressController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all addresses of one supplier.
        /// </summary>
        /// <param name="sid">SupplierId</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Address[]> GetAllAddresses([FromRoute] int sid) {
            return Ok(context.Addresses.Where(v => v.SupplierId == sid).ToArray());
        }

        /// <summary>
        /// Returns the address with a given id of one supplier.
        /// </summary>
        /// <param name="sid">SupplierId</param>
        /// <param name="id">AddressId</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Address> GetAddress([FromRoute] int sid, [FromRoute] int id) {
            var value = context.Addresses.Where(v => (v.SupplierId == sid) && (v.Id == id)).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds an address to one supplier.
        /// </summary>
        /// <param name="sid">SupplierId</param>
        /// <param name="value">new Address</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Address>> AddAddress([FromRoute] int sid, [FromBody] Address value) {
            if (ModelState.IsValid) {
                //test if address already exists
                if (context.Addresses.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "Address already exists");
                    return Conflict(ModelState); //address with id already exists, we return a conflict
                }

                //test if referenced supplier exists
                if (context.Suppliers.Any(s => s.Id == sid) is false) {
                    ModelState.AddModelError("validationError", "Supplier not found");
                    return Conflict(ModelState);
                }

                value.SupplierId = sid; // set reference
                context.Addresses.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the address
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Address
        }
    }
}