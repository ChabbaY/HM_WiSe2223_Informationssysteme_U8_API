using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for offers.
    /// </summary>
    [Route("api/offers")]
    [ApiController]
    public class OfferController : ControllerBase {
        private Context context;
        public OfferController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all offers.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Offer[]> GetAllOffers() {
            return Ok(context.Offers.ToArray());
        }

        /// <summary>
        /// Returns the offer with a given id.
        /// </summary>
        /// <param name="oid">OfferId</param>
        [HttpGet("{oid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Offer> GetOffers([FromRoute] int oid) {
            var value = context.Offers.Where(v => v.Id == oid).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds an offer.
        /// </summary>
        /// <param name="value">new Offer</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Offer>> AddOffer([FromBody] Offer value) {
            if (ModelState.IsValid) {
                //test if offer already exists
                if (context.Offers.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "Offer already exists");
                    return Conflict(ModelState); //offer with id already exists, we return a conflict
                }

                //test if referenced supplier exists
                if (context.Suppliers.Any(s => s.Id == value.SupplierId) is false) {
                    ModelState.AddModelError("validationError", "Supplier not found");
                    return Conflict(ModelState);
                }

                //test if referenced request exists
                if (context.Requests.Any(r => r.Id == value.RequestId) is false) {
                    ModelState.AddModelError("validationError", "Request not found");
                    return Conflict(ModelState);
                }

                context.Offers.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the offer
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Offer
        }

        /// <summary>
        /// Updates an offer.
        /// </summary>
        /// <param name="value">new Offer</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Offer>> UpdateOffer([FromBody] Offer value) {
            if (ModelState.IsValid) {
                var toUpdate = context.Offers.Where(o => o.Id == value.Id).FirstOrDefault();
                if (toUpdate != null) {
                    toUpdate.Date = value.Date;
                    toUpdate.Deadline = value.Deadline;
                    toUpdate.Currency = value.Currency;
                    toUpdate.Comment = value.Comment;
                    toUpdate.SupplierId = value.SupplierId;
                    toUpdate.RequestId = value.RequestId;

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