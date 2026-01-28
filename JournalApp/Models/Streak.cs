using System;
using System.ComponentModel.DataAnnotations;

namespace JournalApp.Models
{
    public class Streak
    {
        [Key]
        public int Id { get; set; }

        public int CurrentStreak { get; set; }

        public int LongestStreak { get; set; }

        public DateTime? LastEntryDate { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
