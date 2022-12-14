using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// This endpoint manages all operations for purchase requisitions.
    /// </summary>
    [Route("api/purchaserequisitions")]
    [ApiController]
    public class PurchaseRequisitionController : ControllerBase {
        private Context context;
        public PurchaseRequisitionController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all purchas requisitions.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Supplier[]> GetAllPurchaseRequisitions() {
            return Ok(context.PurchaseRequisitions.ToArray());
        }

        /// <summary>
        /// Returns the purchase requisition with a given id.
        /// </summary>
        /// <param name="prid">PurchaseRequisitionId</param>
        [HttpGet("{prid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PurchaseRequisition> GetPurchaseRequisition([FromRoute] int prid) {
            var value = context.PurchaseRequisitions.Where(v => v.Id == prid).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a purchase requisition.
        /// </summary>
        /// <param name="value">new Purchase Requisition</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PurchaseRequisition>> AddPurchaseRequisition([FromBody] PurchaseRequisition value) {
            if (ModelState.IsValid) {
                //test if purchase requisition already exists
                if (context.PurchaseRequisitions.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "PurchaseRequisition already exists");
                    return Conflict(ModelState); //purchase requisition with id already exists, we return a conflict
                }

                context.PurchaseRequisitions.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the purchase requisition
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of PurchaseRequisition
        }
    }
}