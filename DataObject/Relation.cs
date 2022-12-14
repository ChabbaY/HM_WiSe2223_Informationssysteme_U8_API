using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Relation {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ContactId { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }
    }
}