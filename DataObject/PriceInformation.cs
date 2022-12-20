using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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

        [IgnoreDataMember]
        [JsonIgnore]
        public double Count { get; set; }//copied from referenced position

        public double Price { get { return UnitPrice * Count; } }
    }
}