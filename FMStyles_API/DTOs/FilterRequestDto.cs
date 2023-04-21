namespace FMStyles_API.DTOs
{
    public class FilterRequestDto
    {
        public string? TextSearch { get; set; } = null;
        public int? Status { get; set; } = null;
        public int? ProvinceId { get; set; } = null;
        public int? Page { get; set; } = 1;
        public int? Limit { get; set; } = 4;

    }
}
