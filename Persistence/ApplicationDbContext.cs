using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Deck> Decks => Set<Deck>();
        public DbSet<Flashcard> Flashcards => Set<Flashcard>();
        public DbSet<UserFlashcardProgress> UserFlashcardProgresses => Set<UserFlashcardProgress>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.DateOfBirth)
                .HasColumnType("timestamp without time zone")
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<UserFlashcardProgress>()
                .HasIndex(p => new { p.UserId, p.FlashcardId })
                .IsUnique();

            modelBuilder.Entity<Deck>()
                .HasOne(d => d.Owner)
                .WithMany(u => u.Decks)
                .HasForeignKey(d => d.OwnerId);

            modelBuilder.Entity<Flashcard>()
                .HasOne(f => f.Deck)
                .WithMany(d => d.Flashcards)
                .HasForeignKey(f => f.DeckId);

            modelBuilder.Entity<UserFlashcardProgress>()
                .HasOne(p => p.User)
                .WithMany(u => u.FlashcardProgresses)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserFlashcardProgress>()
                .HasOne(p => p.Flashcard)
                .WithMany(f => f.Progresses)
                .HasForeignKey(p => p.FlashcardId);
        }
    }
}
