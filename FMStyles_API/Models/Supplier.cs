using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMStyles_API.Models
{
    public class Supplier
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [StringLength(100)]

        public string Email { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int CommuneId { get; set; }
        public string Address { get; set; }
        [StringLength(255)]

        public string Note { get; set; }

        public int Status { get; set; }
        [StringLength(20)]
        public string DebtCode { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        [Required]
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; } = null;
        public int? DeletedBy { get; set; }
        public bool DeleteFlag { get; set; } = false;
        public int? CategoryId { get; set; }
        public SupplierCategory SupplierCategory { get; set; }
    }
}
