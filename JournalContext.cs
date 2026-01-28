using JournalApp.Models;
using Microsoft.EntityFrameworkCore;

namespace JournalApp.Data
{
    public class JournalContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=journal.db");
        }
    }
}