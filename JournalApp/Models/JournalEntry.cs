using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JournalApp.Models
{
    public class JournalEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        // Mood relationships
        [Required]
        public int PrimaryMoodId { get; set; }
        public Mood PrimaryMood { get; set; }

        public int? SecondaryMood1Id { get; set; }
        public Mood SecondaryMood1 { get; set; }

        public int? SecondaryMood2Id { get; set; }
        public Mood SecondaryMood2 { get; set; }

        // Category
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        // Tags - stored as comma-separated IDs
        public string TagIds { get; set; }

        // Word count
        public int WordCount { get; set; }

        [NotMapped]
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
