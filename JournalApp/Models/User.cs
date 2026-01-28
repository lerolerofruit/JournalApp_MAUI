using System;
using System.ComponentModel.DataAnnotations;

namespace JournalApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Theme { get; set; } = "Light"; // Light or Dark
    }
}
