using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Position {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequestId { get; set; }

        [Required]
        public int MaterialId { get; set; }

        [Required]
        public int Pos { get; set; }

        [Required]
        public double Count { get; set; }
    }
}