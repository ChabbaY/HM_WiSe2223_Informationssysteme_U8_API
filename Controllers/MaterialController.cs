using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for materials.
    /// </summary>
    [Route("api/materials")]
    [ApiController]
    public class MaterialController : ControllerBase {
        private Context context;
        public MaterialController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all materials.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Material[]> GetAllMaterials() {
            return Ok(context.Materials.ToArray());
        }

        /// <summary>
        /// Returns the material with a given id.
        /// </summary>
        /// <param name="mid">MaterialId</param>
        [HttpGet("{mid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Material> GetMaterial(int mid) {
            var value = context.Materials.Where(v => v.Id == mid).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a material.
        /// </summary>
        /// <param name="value">new Material</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Material>> AddCustomer([FromBody] Material value) {
            if (ModelState.IsValid) {
                //test if material already exists
                if (context.Materials.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "Material already exists");
                    return Conflict(ModelState); //material with id already exists, we return a conflict
                }

                context.Materials.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the material
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Material
        }

        /// <summary>
        /// Updates a material.
        /// </summary>
        /// <param name="mid">MaterialId</param>
        /// <param name="value">new Material</param>
        [HttpPut("{mid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Material>> UpdateMaterial([FromRoute] int mid, [FromBody] Material value) {
            if (ModelState.IsValid) {
                var toUpdate = context.Materials.Where(m => m.Id == mid).FirstOrDefault();
                if (toUpdate != null) {
                    toUpdate.Name = value.Name;
                    toUpdate.Type = value.Type;

                    await context.SaveChangesAsync();

                    return Ok(value);
                } else {
                    return NotFound(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Delete a material. Blocks if referenced by a Position.
        /// </summary>
        /// <param name="mid">MaterialId</param>
        [HttpDelete("{mid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Material>> DeleteMaterial([FromRoute] int mid) {
            if (context.Positions.Where(p => p.MaterialId == mid).Any()) {
                //block because of reference
                ModelState.AddModelError("referntialIntegrityViolation", "Material refernced by a Position");
                return Conflict(ModelState);
            }

            var toDelete = context.Materials.Where(m => m.Id == mid);
            context.Materials.RemoveRange(toDelete);

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}