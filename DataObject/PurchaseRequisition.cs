using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class PurchaseRequisition {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string NeededUntil { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }
    }
}