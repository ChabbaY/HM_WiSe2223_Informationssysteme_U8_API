using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Material {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Type { get; set; }
    }
}