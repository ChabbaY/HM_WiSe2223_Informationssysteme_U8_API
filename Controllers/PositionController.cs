using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for positions for one purchase requisition.
    /// </summary>
    [Route("api/purchaserequisitions/{prid}/positions")]
    [ApiController]
    public class PositionController : ControllerBase {
        private Context context;
        public PositionController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all positions of one purchase requisition.
        /// </summary>
        /// <param name="prid">PurchaseRequisitionId</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Position[]> GetAllPositions([FromRoute] int prid) {
            return Ok(context.Positions.Where(v => v.PurchaseRequisitionId == prid).ToArray());
        }

        /// <summary>
        /// Returns the position with a given id of one purchase requisition.
        /// </summary>
        /// <param name="prid">PurchaseRequisitionId</param>
        /// <param name="pid">PositionId</param>
        [HttpGet("{pid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Position> GetPosition([FromRoute] int prid, [FromRoute] int pid) {
            var value = context.Positions.Where(v => (v.PurchaseRequisitionId == prid) && (v.Id == pid)).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a position to one purchase requisition.
        /// </summary>
        /// <param name="prid">PurchaseRequisitionId</param>
        /// <param name="value">new Position</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Position>> AddPosition([FromRoute] int prid, [FromBody] Position value) {
            if (ModelState.IsValid) {
                //test if position already exists
                if (context.Positions.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "Position already exits");
                    return Conflict(); //position with id already exists, we return a conflict
                }

                //test if referenced purchase requisition exists
                if (context.PurchaseRequisitions.Any(pr => pr.Id == prid) is false) {
                    ModelState.AddModelError("validationError", "PurchaseRequisition not found");
                    return Conflict(ModelState);
                }

                //test if referenced material exists
                if (context.Materials.Any(m => m.Id == value.MaterialId) is false) {
                    ModelState.AddModelError("validationError", "Material not found");
                    return Conflict(ModelState);
                }

                value.PurchaseRequisitionId = prid; // set reference
                context.Positions.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the position
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Position
        }

        /// <summary>
        /// Updates a position of one purchase requisition.
        /// </summary>
        /// <param name="prid">PurchaseRequisitionId</param>
        /// <param name="pid">PositionId</param>
        /// <param name="value">new Position</param>
        [HttpPut("{pid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Position>> UpdatePosition([FromRoute] int prid, [FromRoute] int pid, [FromBody] Position value) {
            if (ModelState.IsValid) {
                var toUpdate = context.Positions.Where(p => (p.Id == pid) && (p.PurchaseRequisitionId == prid)).FirstOrDefault();
                if (toUpdate != null) {
                    toUpdate.Pos = value.Pos;
                    toUpdate.Count = value.Count;
                    toUpdate.Unit = value.Unit;
                    toUpdate.MaterialId = value.MaterialId;

                    //update count where reference is set
                    foreach (PriceInformation x in context.PriceInformation.Where(pi => pi.PositionId == pid)) {
                        x.Count = value.Count;
                    }

                    await context.SaveChangesAsync();

                    return Ok(value);
                } else {
                    return NotFound(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Delete a position of one purchase requisition. Blocks if referenced by a PriceInformation.
        /// </summary>
        /// <param name="prid">PurchaseRequisitionId</param>
        /// <param name="pid">PositionId</param>
        [HttpDelete("{pid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Position>> DeletePosition([FromRoute] int prid, [FromRoute] int pid) {
            if (context.PriceInformation.Where(pi => pi.PositionId == pid).Any()) {
                //block because of reference
                ModelState.AddModelError("referntialIntegrityViolation", "Position refernced by a PriceInformation");
                return Conflict(ModelState);
            }

            var toDelete = context.Positions.Where(p => (p.Id == pid) && (p.PurchaseRequisitionId == prid));
            context.Positions.RemoveRange(toDelete);

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}