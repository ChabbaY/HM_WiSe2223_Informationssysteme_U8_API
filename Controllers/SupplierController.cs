﻿using System.Linq;
using System.Threading.Tasks;
using API.DataObject;
using API.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    /// <summary>
    /// This endpoint manages all operations for suppliers
    /// </summary>
    [Route("api/suppliers")]
    [ApiController]
    public class SupplierController : ControllerBase {
        private Context context;
        public SupplierController(Context context) {
            this.context = context;
        }

        /// <summary>
        /// Returns all suppliers.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<Supplier[]> GetAllSuppliers() {
            return Ok(context.Suppliers.ToArray());
        }

        /// <summary>
        /// Returns the supplier with a given id.
        /// </summary>
        /// <param name="sid">SupplierId</param>
        [HttpGet("{sid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Supplier> GetSupplier([FromRoute] int sid) {
            var value = context.Suppliers.Where(v => v.Id == sid).FirstOrDefault();
            if (value == null) return NotFound();
            return Ok(value);
        }

        /// <summary>
        /// Adds a supplier.
        /// </summary>
        /// <param name="value">new Supplier</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Supplier>> AddSupplier([FromBody] Supplier value) {
            if (ModelState.IsValid) {
                //test if supplier already exists
                if (context.Suppliers.Where(v => v.Id == value.Id).FirstOrDefault() != null) {
                    ModelState.AddModelError("validationError", "Supplier already exists");
                    return Conflict(ModelState); //supplier with id already exists, we return a conflict
                }

                context.Suppliers.Add(value);
                await context.SaveChangesAsync();

                return Ok(value); //we return the supplier
            }
            return BadRequest(ModelState); //Model is not valid -> Validation Annotation of Supplier
        }
    }
}