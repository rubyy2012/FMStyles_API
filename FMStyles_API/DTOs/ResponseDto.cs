namespace FMStyles_API.DTOs
{
    public class ResponseDto
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public bool isSuccess { get; set; } = true;
    }
}
