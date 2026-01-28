using JournalApp.Data;
using JournalApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JournalApp.Services
{
    public class JournalService
    {
        private readonly JournalContext _context;

        public JournalService(JournalContext context)
        {
            _context = context;
        }

        public async Task<List<JournalEntry>> GetEntriesAsync(int userId)
        {
            return await _context.JournalEntries
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task AddEntryAsync(JournalEntry entry)
        {
            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEntryAsync(JournalEntry entry)
        {
            _context.JournalEntries.Update(entry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEntryAsync(int entryId)
        {
            var entry = await _context.JournalEntries.FindAsync(entryId);
            if (entry != null)
            {
                _context.JournalEntries.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}