using System.ComponentModel.DataAnnotations;

namespace JournalApp.Models
{
    public class Mood
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; } // Positive, Neutral, Negative
    }
}
