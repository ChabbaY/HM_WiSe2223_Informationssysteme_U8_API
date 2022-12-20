using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace API.DataObject {
    public class Offer {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Date { get; set; }

        [MaxLength(100)]
        public string Deadline { get; set; }

        [MaxLength(100)]
        public string Currency { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int RequestId { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public double PricesSum { get; set; }//copied from referenced price information
        
        public double Price {
            get {
                return PricesSum;
            }
        }
    }
}