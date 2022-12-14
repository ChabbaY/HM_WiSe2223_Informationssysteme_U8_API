using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Position {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int Pos { get; set; }

        [Required]
        public double Count { get; set; }

        [MaxLength(100)]
        public string Unit { get; set; }

        [Required]
        public int PurchaseRequisitionId { get; set; }

        [Required]
        public int MaterialId { get; set; }
    }
}