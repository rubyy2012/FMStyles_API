using System.ComponentModel.DataAnnotations;

namespace FMStyles_API.DTOs
{
    public class SupplierCategoryDto
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
    }
}
