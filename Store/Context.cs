using API.DataObject;
using Microsoft.EntityFrameworkCore;

namespace API.Store {
    /// <summary>
    /// Database context that allows us to specify the tables of a database
    /// </summary>
    public class Context : DbContext {
        public Context(DbContextOptions<Context> options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder builder) {
            //additional settings for certain entity options
        }

        /// <summary>
        /// database table Addresses
        /// </summary>
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// database table Materials
        /// </summary>
        public DbSet<Material> Materials { get; set; }

        /// <summary>
        /// database table Materials
        /// </summary>
        public DbSet<Offer> Offers { get; set; }

        /// <summary>
        /// database table Positions
        /// </summary>
        public DbSet<Position> Positions { get; set; }

        /// <summary>
        /// database table Positions
        /// </summary>
        public DbSet<PriceInformation> PriceInformation { get; set; }

        /// <summary>
        /// database table Positions
        /// </summary>
        public DbSet<PurchaseRequisition> PurchaseRequisitions { get; set; }

        /// <summary>
        /// database table Requests
        /// </summary>
        public DbSet<Request> Requests { get; set; }

        /// <summary>
        /// database table Supplier
        /// </summary>
        public DbSet<Supplier> Suppliers { get; set; }
    }
}