using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Address {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AdderssInformationId { get; set; }

        [MaxLength(100)]
        public string Street { get; set; }
        
        [MaxLength(100)]
        public string House_Nr { get; set; }

        [MaxLength(100)]
        public string Postcode { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string Country { get; set; }

        [MaxLength(100)]
        public string Region { get; set; }

        [MaxLength(100)]
        public string Timezone { get; set; }
    }
}