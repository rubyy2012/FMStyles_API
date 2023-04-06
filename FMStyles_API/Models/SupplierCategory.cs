using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMStyles_API.Models
{
    public class SupplierCategory
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        public int? ParentId { get; set; }
        [StringLength(255)]
        public string Note { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        [Required]
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; } = null;
        public int DeletedBy { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }
    }
}
