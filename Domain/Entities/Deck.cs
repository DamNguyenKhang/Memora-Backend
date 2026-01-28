using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Deck : IEntity<long>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long OwnerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public List<string>? Tags { get; set; } = new();

        public bool IsPublic { get; set; } = false;

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public long? FolderId { get; set; }

        // Navigation
        [ForeignKey(nameof(FolderId))]
        public Folder? Folder { get; set; }
        public User Owner { get; set; } = null!;
        public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
    }
}
