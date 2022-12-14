using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Request {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [MaxLength(100)]
        public string FromDate { get; set; }

        [MaxLength(100)]
        public string UntilDate { get; set; }
    }
}