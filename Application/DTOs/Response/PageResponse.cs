namespace Application.DTOs.Response
{
    public class PageResponse
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / Size);
    }
}
