using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Flashcard : IEntity<long>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public long DeckId { get; set; }

    [Required]
    public string Front { get; set; } = null!;

    [Required]
    public string Back { get; set; } = null!;

    public string? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    // Navigation
    public Deck Deck { get; set; } = null!;
    public ICollection<UserFlashcardProgress> Progresses { get; set; } = new List<UserFlashcardProgress>();
}

}