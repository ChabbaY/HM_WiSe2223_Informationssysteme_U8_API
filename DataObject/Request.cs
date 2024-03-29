﻿using System.ComponentModel.DataAnnotations;

namespace API.DataObject {
    public class Request {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Date { get; set; }

        [MaxLength(100)]
        public string Deadline { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }

        [Required]
        public int PurchaseRequisitionId { get; set; }
    }
}