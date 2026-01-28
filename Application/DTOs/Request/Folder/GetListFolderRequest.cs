namespace Application.DTOs.Request.Folder
{
    public class GetListFolderRequest : PageRequest
    {
        public string? Name { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}