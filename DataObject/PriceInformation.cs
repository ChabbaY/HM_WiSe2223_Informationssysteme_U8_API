using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class PriceInformation {
        [Key]
        public int Id { get; set; }

        [Required]
        public double UnitPrice { get; set; }

        [Required]
        public int OfferId { get; set; }

        [Required]
        public int PositionId { get; set; }
    }
}