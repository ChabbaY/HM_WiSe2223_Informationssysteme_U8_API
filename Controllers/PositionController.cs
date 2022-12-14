using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for positions for one request.
    /// </summary>
    [Route("api/requests/{rid}/positions")]
    [ApiController]
    public class PositionController : ControllerBase {
        private Context context;
        public PositionController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all positions.
        /// </summary>
        /// <param name="rid"></param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Position[]> GetAllPositions([FromRoute] int rid) {
            return Ok(context.Positions.Where(v => v.RequestId == rid).ToArray());
        }

        /// <summary>
        /// Returns the position with a given id.
        /// </summary>
        /// <param name="rid">RequestId</param>
        /// <param name="pid">PositionId</param>
        [HttpGet("{pid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Position> GetPosition([FromRoute] int rid, [FromRoute] int pid) {
            var value = context.Positions.Where(v => (v.RequestId == rid) && (v.Id == pid)).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a position.
        /// </summary>
        /// <param name="rid">RequestId</param>
        /// <param name="value">new Position</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Position>> AddPosition([FromRoute] int rid, [FromBody] Position value) {
            if (ModelState.IsValid) {
                //test if position already exists
                if (context.Positions.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "Position already exits");
                    return Conflict(); //position with id already exists, we return a conflict
                }

                //test if referenced request exists
                if (context.Requests.Any(r => r.Id == rid) is false)
                {
                    ModelState.AddModelError("validationError", "Request not found");
                    return NotFound(ModelState);
                }

                //test if referenced material exists
                if (context.Materials.Any(m => m.Id == value.MaterialId) is false)
                {
                    ModelState.AddModelError("validationError", "Material not found");
                    return NotFound(ModelState);
                }

                value.RequestId = rid; // set reference
                context.Positions.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the position
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Position
        }
    }
}