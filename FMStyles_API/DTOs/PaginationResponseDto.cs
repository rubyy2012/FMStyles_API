using FMStyles_API.Models;
using System.Collections.Generic;

namespace FMStyles_API.DTOs
{
    public class PaginationResponseDto
    {
        public PaginationResponseDto()
        {
            
        }
        public int MaxItem { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int TotalPages { get; set; }
        public string Message { get; set; }
        public IEnumerable<Supplier> Suppliers { get; set; }
    }
}
