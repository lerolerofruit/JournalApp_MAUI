using Microsoft.EntityFrameworkCore;
using JournalApp.Models;
using System;

namespace JournalApp.Data
{
    public class JournalContext : DbContext
    {
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<Mood> Moods { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Streak> Streaks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "JournalApp",
                "journal.db"
            );
            
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(dbPath));
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure JournalEntry
            modelBuilder.Entity<JournalEntry>()
                .HasIndex(e => e.Date)
                .IsUnique();

            // Configure relationships
            modelBuilder.Entity<JournalEntry>()
                .HasOne(e => e.PrimaryMood)
                .WithMany()
                .HasForeignKey(e => e.PrimaryMoodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JournalEntry>()
                .HasOne(e => e.SecondaryMood1)
                .WithMany()
                .HasForeignKey(e => e.SecondaryMood1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JournalEntry>()
                .HasOne(e => e.SecondaryMood2)
                .WithMany()
                .HasForeignKey(e => e.SecondaryMood2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JournalEntry>()
                .HasOne(e => e.Category)
                .WithMany(c => c.JournalEntries)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed default moods
            SeedMoods(modelBuilder);
            
            // Seed default tags
            SeedTags(modelBuilder);
            
            // Seed default categories
            SeedCategories(modelBuilder);
        }

        private void SeedMoods(ModelBuilder modelBuilder)
        {
            var moods = new[]
            {
                // Positive
                new Mood { Id = 1, Name = "Happy", Category = "Positive" },
                new Mood { Id = 2, Name = "Excited", Category = "Positive" },
                new Mood { Id = 3, Name = "Relaxed", Category = "Positive" },
                new Mood { Id = 4, Name = "Grateful", Category = "Positive" },
                new Mood { Id = 5, Name = "Confident", Category = "Positive" },
                // Neutral
                new Mood { Id = 6, Name = "Calm", Category = "Neutral" },
                new Mood { Id = 7, Name = "Thoughtful", Category = "Neutral" },
                new Mood { Id = 8, Name = "Curious", Category = "Neutral" },
                new Mood { Id = 9, Name = "Nostalgic", Category = "Neutral" },
                new Mood { Id = 10, Name = "Bored", Category = "Neutral" },
                // Negative
                new Mood { Id = 11, Name = "Sad", Category = "Negative" },
                new Mood { Id = 12, Name = "Angry", Category = "Negative" },
                new Mood { Id = 13, Name = "Stressed", Category = "Negative" },
                new Mood { Id = 14, Name = "Lonely", Category = "Negative" },
                new Mood { Id = 15, Name = "Anxious", Category = "Negative" }
            };

            modelBuilder.Entity<Mood>().HasData(moods);
        }

        private void SeedTags(ModelBuilder modelBuilder)
        {
            var tags = new[]
            {
                new Tag { Id = 1, Name = "Work" },
                new Tag { Id = 2, Name = "Career" },
                new Tag { Id = 3, Name = "Studies" },
                new Tag { Id = 4, Name = "Family" },
                new Tag { Id = 5, Name = "Friends" },
                new Tag { Id = 6, Name = "Relationships" },
                new Tag { Id = 7, Name = "Health" },
                new Tag { Id = 8, Name = "Fitness" },
                new Tag { Id = 9, Name = "Personal Growth" },
                new Tag { Id = 10, Name = "Self-care" },
                new Tag { Id = 11, Name = "Hobbies" },
                new Tag { Id = 12, Name = "Travel" },
                new Tag { Id = 13, Name = "Nature" },
                new Tag { Id = 14, Name = "Finance" },
                new Tag { Id = 15, Name = "Spirituality" },
                new Tag { Id = 16, Name = "Birthday" },
                new Tag { Id = 17, Name = "Holiday" },
                new Tag { Id = 18, Name = "Vacation" },
                new Tag { Id = 19, Name = "Celebration" },
                new Tag { Id = 20, Name = "Exercise" },
                new Tag { Id = 21, Name = "Reading" },
                new Tag { Id = 22, Name = "Writing" },
                new Tag { Id = 23, Name = "Cooking" },
                new Tag { Id = 24, Name = "Meditation" },
                new Tag { Id = 25, Name = "Yoga" },
                new Tag { Id = 26, Name = "Music" },
                new Tag { Id = 27, Name = "Shopping" },
                new Tag { Id = 28, Name = "Parenting" },
                new Tag { Id = 29, Name = "Projects" },
                new Tag { Id = 30, Name = "Planning" },
                new Tag { Id = 31, Name = "Reflection" }
            };

            modelBuilder.Entity<Tag>().HasData(tags);
        }

        private void SeedCategories(ModelBuilder modelBuilder)
        {
            var categories = new[]
            {
                new Category { Id = 1, Name = "Personal" },
                new Category { Id = 2, Name = "Professional" },
                new Category { Id = 3, Name = "Health & Wellness" },
                new Category { Id = 4, Name = "Relationships" },
                new Category { Id = 5, Name = "Goals & Dreams" }
            };

            modelBuilder.Entity<Category>().HasData(categories);
        }
    }
}
