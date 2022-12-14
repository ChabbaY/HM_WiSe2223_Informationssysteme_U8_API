using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Supplier {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Telephone { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Language { get; set; }
    }
}