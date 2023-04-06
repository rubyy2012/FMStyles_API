using FMStyles_API.Models;
using System.ComponentModel.DataAnnotations;

namespace FMStyles_API.DTOs
{
    public class SupplierResponseDto
    {
        public int Id { get; set; }
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
        public int Status { get; set; }
        [StringLength(20)]
        public string DebtCode { get; set; }
        public int? CategoryId { get; set; }
        public SupplierCategory SupplierCategory { get; set; }
    }
}
