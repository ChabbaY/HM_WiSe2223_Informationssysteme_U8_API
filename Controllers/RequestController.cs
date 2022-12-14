using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for requests for one customer.
    /// </summary>
    [Route("api/customers/{cid}/requests")]
    [ApiController]
    public class RequestController : ControllerBase {
        private Context context;
        public RequestController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all requests.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Request[]> GetAllRequests([FromRoute] int cid) {
            return Ok(context.Requests.Where(v => v.CustomerId == cid).ToArray());
        }

        /// <summary>
        /// Returns the request with a given id.
        /// </summary>
        /// <param name="cid">CustomerId</param>
        /// <param name="rid">RequestId</param>
        [HttpGet("{rid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Request> GetRequest([FromRoute] int cid, [FromRoute] int rid) {
            var value = context.Requests.Where(v => (v.CustomerId == cid) && (v.Id == rid)).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a request.
        /// </summary>
        /// <param name="cid">CustomerId</param>
        /// <param name="value">new Request</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Request>> AddRequest([FromRoute] int cid, [FromBody] Request value) {
            if (ModelState.IsValid) {
                //test if request already exists
                if (context.Requests.Where(v => v.Id == value.Id).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("validationError", "Request already exists");
                    return Conflict(ModelState); //request with id already exists, we return a conflict
                }

                //test if referenced customer exists
                if (context.Customers.Any(c => c.Id == cid) is false) {
                    ModelState.AddModelError("validationError", "Customer not found");
                    return NotFound(ModelState);
                }

                value.CustomerId = cid; // set reference
                context.Requests.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the request
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Request
        }
    }
}