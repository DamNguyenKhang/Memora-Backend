namespace Application.DTOs.Request
{
    public class PageRequest
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
