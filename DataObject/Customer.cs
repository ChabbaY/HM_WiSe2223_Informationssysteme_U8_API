using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Customer {
        [Key]
        public int Id { get; set; }

        public int AddressInformationId { get; set; }
    }
}