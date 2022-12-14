using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for requests.
    /// </summary>
    [Route("api/requests")]
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
        public ActionResult<Request[]> GetAllRequests() {
            return Ok(context.Requests.ToArray());
        }

        /// <summary>
        /// Returns the request with a given id.
        /// </summary>
        /// <param name="rid">RequestId</param>
        [HttpGet("{rid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Request> GetRequest([FromRoute] int rid) {
            var value = context.Requests.Where(v => v.Id == rid).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a request.
        /// </summary>
        /// <param name="value">new Request</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Request>> AddRequest([FromBody] Request value) {
            if (ModelState.IsValid) {
                //test if request already exists
                if (context.Requests.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "Request already exists");
                    return Conflict(ModelState); //request with id already exists, we return a conflict
                }

                //test if referenced purchase requisition exists
                if (context.PurchaseRequisitions.Any(pr => pr.Id == value.PurchaseRequisitionId) is false) {
                    ModelState.AddModelError("validationError", "PurchaseRequisition not found");
                    return Conflict(ModelState);
                }

                context.Requests.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the request
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Request
        }

        /// <summary>
        /// Updates a request.
        /// </summary>
        /// <param name="rid">RequestId</param>
        /// <param name="value">new Request</param>
        [HttpPut("{rid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Request>> UpdateRequest([FromRoute] int rid, [FromBody] Request value) {
            if (ModelState.IsValid) {
                var toUpdate = context.Requests.Where(r => r.Id == rid).FirstOrDefault();
                if (toUpdate != null) {
                    toUpdate.Date = value.Date;
                    toUpdate.Deadline = value.Deadline;
                    toUpdate.Comment = value.Comment;
                    toUpdate.PurchaseRequisitionId = value.PurchaseRequisitionId;

                    await context.SaveChangesAsync();

                    return Ok(value);
                } else {
                    return NotFound(ModelState);
                }
            }
            return BadRequest(ModelState);
        }
    }
}