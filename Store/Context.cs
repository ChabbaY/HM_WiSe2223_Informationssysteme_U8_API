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
        /// database table AddressInformation
        /// </summary>
        public DbSet<AddressInformation> AddressInformation { get; set; }

        /// <summary>
        /// database table Customers
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// database table Contacts
        /// </summary>
        public DbSet<Contact> Contacts { get; set; }

        /// <summary>
        /// database table Materials
        /// </summary>
        public DbSet<Material> Materials { get; set; }

        /// <summary>
        /// database table Positions
        /// </summary>
        public DbSet<Position> Positions { get; set; }

        /// <summary>
        /// database table Relations
        /// </summary>
        public DbSet<Relation> Relations { get; set; }

        /// <summary>
        /// database table Requests
        /// </summary>
        public DbSet<Request> Requests { get; set; }
    }
}