using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs.Request.Deck
{
    public class CreateDeckMultipartRequest
    {
        [Required]
        [FromForm(Name = "deck")]
        public string Deck { get; set; } = null!;

        [FromForm(Name = "images")]
        public List<IFormFile>? Images { get; set; } = new();
    }
}