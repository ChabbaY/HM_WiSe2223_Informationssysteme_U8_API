using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for relations.
    /// </summary>
    [Route("api/relations")]
    [ApiController]
    public class RelationController : ControllerBase {
        private Context context;
        public RelationController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all relations.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Relation[]> GetAllRelations() {
            return Ok(context.Relations.ToArray());
        }

        /// <summary>
        /// Returns the relation with a given id.
        /// </summary>
        /// <param name="rid">RelationId</param>
        [HttpGet("{rid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Relation> GetRelation([FromRoute] int rid) {
            var value = context.Relations.Where(v => v.Id == rid).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a relation.
        /// </summary>
        /// <param name="value">new Relation</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Relation>> AddCustomer([FromBody] Relation value) {
            if (ModelState.IsValid) {
                //test if relation already exists
                if (context.Relations.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "Relation already exists");
                    return Conflict(ModelState); //relation with id already exists, we return a conflict
                }

                //test if referenced customer exists
                if (context.Customers.Any(c => c.Id == value.CustomerId) is false)
                {
                    ModelState.AddModelError("validationError", "Customer not found");
                    return NotFound(ModelState);
                }

                //test if referenced contact exists
                if (context.Contacts.Any(c => c.Id == value.ContactId) is false)
                {
                    ModelState.AddModelError("validationError", "Contact not found");
                    return NotFound(ModelState);
                }

                context.Relations.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the relation
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Relation
        }
    }
}