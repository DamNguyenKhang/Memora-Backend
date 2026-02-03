namespace Application.DTOs.Request.UserProgress
{
    public class GetListDeckProcessRequest : PageRequest
    {
        public long FolderId { get; set; }
    }
}