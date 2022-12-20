using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for price information for one offer.
    /// </summary>
    [Route("api/offer/{oid}/priceinformation")]
    [ApiController]
    public class PriceInformationController : ControllerBase {
        private Context context;
        public PriceInformationController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all price information of one offer.
        /// </summary>
        /// <param name="oid">OfferId</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<PriceInformation[]> GetAllPriceInformation([FromRoute] int oid) {
            return Ok(context.PriceInformation.Where(v => v.OfferId == oid).ToArray());
        }

        /// <summary>
        /// Returns the price information with a given id of one offer.
        /// </summary>
        /// <param name="oid">OfferId</param>
        /// <param name="piid">PriceInformationId</param>
        [HttpGet("{piid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PriceInformation> GetPriceInformation([FromRoute] int oid, [FromRoute] int piid) {
            var value = context.PriceInformation.Where(v => (v.OfferId == oid) && (v.Id == piid)).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a price information to one offer.
        /// </summary>
        /// <param name="oid">OfferId</param>
        /// <param name="value">new PriceInformation</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PriceInformation>> AddPriceInformation([FromRoute] int oid, [FromBody] PriceInformation value) {
            if (ModelState.IsValid) {
                //test if price information already exists
                if (context.PriceInformation.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "Price Information already exits");
                    return Conflict(); //price information with id already exists, we return a conflict
                }

                //test if referenced offer exists
                if (context.Offers.Any(o => o.Id == oid) is false) {
                    ModelState.AddModelError("validationError", "Offer not found");
                    return Conflict(ModelState);
                }

                //test if referenced position exists
                if (context.Positions.Any(p => p.Id == value.PositionId) is false) {
                    ModelState.AddModelError("validationError", "Position not found");
                    return Conflict(ModelState);
                }

                //copy count from position
                value.Count = context.Positions.Where(p => p.Id == value.PositionId).FirstOrDefault().Count;

                value.OfferId = oid; // set reference
                context.PriceInformation.Add(value);
                await context.SaveChangesAsync();

                await UpdateOffer(oid);

                return Ok(value); //we return the price information
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of PriceInformation
        }

        /// <summary>
        /// Updates a price information of one offer.
        /// </summary>
        /// <param name="oid">OfferId</param>
        /// <param name="piid">PriceInformationId</param>
        /// <param name="value">new PriceInformation</param>
        [HttpPut("{piid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PriceInformation>> UpdatePriceInformation([FromRoute] int oid, [FromRoute] int piid, [FromBody] PriceInformation value) {
            if (ModelState.IsValid) {
                var toUpdate = context.PriceInformation.Where(pi => (pi.Id == piid) && (pi.OfferId == oid)).FirstOrDefault();
                if (toUpdate != null) {
                    toUpdate.UnitPrice = value.UnitPrice;
                    toUpdate.PositionId = value.PositionId;

                    //copy count from position
                    toUpdate.Count = context.Positions.Where(p => p.Id == value.PositionId).FirstOrDefault().Count;

                    await context.SaveChangesAsync();

                    await UpdateOffer(oid);

                    return Ok(value);
                } else {
                    return NotFound(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Delete a price information of one offer.
        /// </summary>
        /// <param name="oid">OfferId</param>
        /// <param name="piid">PriceInformationId</param>
        [HttpDelete("{piid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PriceInformation>> DeletePriceInformation([FromRoute] int oid, [FromRoute] int piid) {
            var toDelete = context.PriceInformation.Where(pi => (pi.Id == piid) && (pi.OfferId == oid));
            context.PriceInformation.RemoveRange(toDelete);

            await context.SaveChangesAsync();

            await UpdateOffer(oid);

            return Ok();
        }

        private async Task UpdateOffer(int offerId) {
            //update offer (prices); after save to include current
            context.Offers.Where(o => o.Id == offerId).FirstOrDefault().PricesSum =
                CalculatePricesSum(offerId);

            await context.SaveChangesAsync();
        }

        private double CalculatePricesSum(int offerId) {
            double result = 0d;
            foreach (PriceInformation x in context.PriceInformation.Where(pi => pi.OfferId == offerId)) {
                result += x.Price;
            }
            return result;
        }
    }
}