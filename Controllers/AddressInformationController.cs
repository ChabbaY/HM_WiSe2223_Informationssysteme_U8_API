using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for address information.
    /// </summary>
    [Route("api/addressinformation")]
    [ApiController]
    public class AddressInformationController : ControllerBase {
        private Context context;
        public AddressInformationController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all address information.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<AddressInformation[]> GetAllAddressInformation() {
            return Ok(context.AddressInformation.ToArray());
        }

        /// <summary>
        /// Returns the address information with a given id.
        /// </summary>
        /// <param name="aid">AddressInformationId</param>
        [HttpGet("{aid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AddressInformation> GetAddressInformation([FromRoute] int aid) {
            var value = context.AddressInformation.Where(v => v.Id == aid).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds an address information.
        /// </summary>
        /// <param name="value">new AddressInformation</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AddressInformation>> AddAddressInformation([FromBody] AddressInformation value) {
            if (ModelState.IsValid) {
                //test if address information already exists
                if (context.AddressInformation.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "AddressInformation already exists");
                    return Conflict(ModelState); //address information with id already exists, we return a conflict
                }

                context.AddressInformation.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the address information
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Address Information
        }
    }
}