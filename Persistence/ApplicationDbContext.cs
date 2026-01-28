using System.Text.Json;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.HasIndex(u => u.Username)
                    .IsUnique();

                entity.Property(u => u.DateOfBirth)
                    .HasColumnType("timestamp without time zone")
                    .IsRequired(false);

                entity.Property(u => u.Role)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();
            });

            modelBuilder.Entity<UserFlashcardProgress>()
                .HasIndex(p => new { p.UserId, p.FlashcardId })
                .IsUnique();

            modelBuilder.Entity<Deck>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.HasOne(d => d.Owner)
                    .WithMany(u => u.Decks)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Folder)
                    .WithMany(f => f.Decks)
                    .HasForeignKey(d => d.FolderId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasQueryFilter(d => !d.IsDeleted);

                entity.Property(d => d.Tags)
                    .HasColumnType("jsonb");
            });

            modelBuilder.Entity<Flashcard>(entity =>
            {
                entity.HasOne(f => f.Deck)
                    .WithMany(d => d.Flashcards)
                    .HasForeignKey(f => f.DeckId);

                entity.OwnsOne(e => e.Front, f =>
                {
                    f.Property(p => p.Text)
                    .HasColumnName("FrontText")
                    .IsRequired();

                    f.Property(p => p.ImageUrl)
                    .HasColumnName("FrontImageUrl");

                    f.Property(p => p.AudioUrl)
                    .HasColumnName("FrontAudioUrl");
                });

                entity.OwnsOne(e => e.Back, b =>
                {
                    b.Property(p => p.Text)
                    .HasColumnName("BackText")
                    .IsRequired();

                    b.Property(p => p.AudioUrl)
                    .HasColumnName("BackAudioUrl");

                    b.Ignore(p => p.ImageUrl);
                });

                entity.Property(f => f.Difficulty)
                    .HasConversion<int>()
                    .HasDefaultValue(FlashcardDifficulty.Medium)
                    .IsRequired();

                entity.HasQueryFilter(f => !f.IsDeleted);
            });

            modelBuilder.Entity<Folder>()
                .HasQueryFilter(f => !f.IsDeleted);


            modelBuilder.Entity<UserFlashcardProgress>()
                .HasOne(p => p.User)
                .WithMany(u => u.FlashcardProgresses)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserFlashcardProgress>()
                .HasOne(p => p.Flashcard)
                .WithMany(f => f.Progresses)
                .HasForeignKey(p => p.FlashcardId);

            modelBuilder.Entity<RefreshToken>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.HasIndex(x => x.Token).IsUnique();

                builder.Property(x => x.Token)
                    .IsRequired()
                    .HasMaxLength(44);

                builder.HasOne(x => x.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<EmailVerification>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.Token).IsUnique();

                entity.HasOne(x => x.User)
                    .WithMany(u => u.EmailVerifications)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(x => x.Token)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(x => x.ExpiredAt)
                    .IsRequired();
            });
        }
    }
}
