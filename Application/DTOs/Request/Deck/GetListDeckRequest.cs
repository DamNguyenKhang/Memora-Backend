namespace Application.DTOs.Request.Deck
{
    public class GetListDeckRequest : PageRequest
    {
        public string? Keyword { get; set; }     
        public bool? IsPublic { get; set; }       
        public DateTime? FromDate { get; set; }  
        public DateTime? ToDate { get; set; }
    }
}