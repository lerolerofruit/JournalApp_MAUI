using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JournalApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<JournalEntry> JournalEntries { get; set; }
    }
}
