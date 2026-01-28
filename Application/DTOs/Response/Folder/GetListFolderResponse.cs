namespace Application.DTOs.Response.Folder
{
    public class GetListFolderResponse : PageResponse
    {
        public IEnumerable<FolderResponse>? Folders { get; set; }
    }
}