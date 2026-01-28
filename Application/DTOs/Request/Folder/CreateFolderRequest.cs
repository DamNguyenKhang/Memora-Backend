using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Folder
{
    public class CreateFolderRequest
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}