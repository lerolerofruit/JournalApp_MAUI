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

        public async Task<JournalEntry> CreateEntryAsync(JournalEntry entry)
        {

            var existingEntry = await _context.JournalEntries
                .FirstOrDefaultAsync(e => e.Date.Date == entry.Date.Date);

            if (existingEntry != null)
            {
                throw new InvalidOperationException("An entry already exists for this date.");
            }

            entry.CreatedAt = DateTime.Now;
            entry.UpdatedAt = DateTime.Now;
            entry.WordCount = CountWords(entry.Content);

            _context.JournalEntries.Add(entry);
            await _context.SaveChangesAsync();

            await UpdateStreakAsync(entry.Date);

            return entry;
        }

        public async Task<JournalEntry> UpdateEntryAsync(JournalEntry entry)
        {
            var existingEntry = await _context.JournalEntries.FindAsync(entry.Id);
            if (existingEntry == null)
            {
                throw new InvalidOperationException("Entry not found.");
            }

            existingEntry.Title = entry.Title;
            existingEntry.Content = entry.Content;
            existingEntry.PrimaryMoodId = entry.PrimaryMoodId;
            existingEntry.SecondaryMood1Id = entry.SecondaryMood1Id;
            existingEntry.SecondaryMood2Id = entry.SecondaryMood2Id;
            existingEntry.CategoryId = entry.CategoryId;
            existingEntry.TagIds = entry.TagIds;
            existingEntry.UpdatedAt = DateTime.Now;
            existingEntry.WordCount = CountWords(entry.Content);

            await _context.SaveChangesAsync();
            return existingEntry;
        }

        public async Task DeleteEntryAsync(int entryId)
        {
            var entry = await _context.JournalEntries.FindAsync(entryId);
            if (entry != null)
            {
                _context.JournalEntries.Remove(entry);
                await _context.SaveChangesAsync();
                await RecalculateStreaksAsync();
            }
        }

        public async Task<JournalEntry> GetEntryByDateAsync(DateTime date)
        {
            return await _context.JournalEntries
                .Include(e => e.PrimaryMood)
                .Include(e => e.SecondaryMood1)
                .Include(e => e.SecondaryMood2)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Date.Date == date.Date);
        }

        public async Task<JournalEntry> GetEntryByIdAsync(int id)
        {
            return await _context.JournalEntries
                .Include(e => e.PrimaryMood)
                .Include(e => e.SecondaryMood1)
                .Include(e => e.SecondaryMood2)
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<JournalEntry>> GetAllEntriesAsync()
        {
            return await _context.JournalEntries
                .Include(e => e.PrimaryMood)
                .Include(e => e.SecondaryMood1)
                .Include(e => e.SecondaryMood2)
                .Include(e => e.Category)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<List<JournalEntry>> GetEntriesPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _context.JournalEntries
                .Include(e => e.PrimaryMood)
                .Include(e => e.SecondaryMood1)
                .Include(e => e.SecondaryMood2)
                .Include(e => e.Category)
                .OrderByDescending(e => e.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalEntriesCountAsync()
        {
            return await _context.JournalEntries.CountAsync();
        }

        public async Task<List<JournalEntry>> SearchEntriesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllEntriesAsync();
            }

            return await _context.JournalEntries
                .Include(e => e.PrimaryMood)
                .Include(e => e.SecondaryMood1)
                .Include(e => e.SecondaryMood2)
                .Include(e => e.Category)
                .Where(e => e.Title.Contains(searchTerm) || e.Content.Contains(searchTerm))
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<List<JournalEntry>> FilterEntriesAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            List<int> moodIds = null,
            List<int> tagIds = null)
        {
            var query = _context.JournalEntries
                .Include(e => e.PrimaryMood)
                .Include(e => e.SecondaryMood1)
                .Include(e => e.SecondaryMood2)
                .Include(e => e.Category)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(e => e.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.Date <= endDate.Value);
            }

            if (moodIds != null && moodIds.Any())
            {
                query = query.Where(e =>
                    moodIds.Contains(e.PrimaryMoodId) ||
                    (e.SecondaryMood1Id.HasValue && moodIds.Contains(e.SecondaryMood1Id.Value)) ||
                    (e.SecondaryMood2Id.HasValue && moodIds.Contains(e.SecondaryMood2Id.Value)));
            }

            if (tagIds != null && tagIds.Any())
            {
                query = query.Where(e => tagIds.Any(tagId => e.TagIds.Contains(tagId.ToString())));
            }

            return await query.OrderByDescending(e => e.Date).ToListAsync();
        }

        public async Task<List<Mood>> GetAllMoodsAsync()
        {
            return await _context.Moods.OrderBy(m => m.Category).ThenBy(m => m.Name).ToListAsync();
        }

        public async Task<Mood> GetMoodByIdAsync(int id)
        {
            return await _context.Moods.FindAsync(id);
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.OrderBy(t => t.Name).ToListAsync();
        }

        public async Task<Tag> CreateCustomTagAsync(string tagName)
        {
            var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
            if (existingTag != null)
            {
                return existingTag;
            }

            var tag = new Tag { Name = tagName, IsCustom = true };
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<List<Tag>> GetTagsByIdsAsync(string tagIds)
        {
            if (string.IsNullOrWhiteSpace(tagIds))
            {
                return new List<Tag>();
            }

            var ids = tagIds.Split(',').Select(int.Parse).ToList();
            return await _context.Tags.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        }

        private async Task UpdateStreakAsync(DateTime entryDate)
        {
            var streak = await _context.Streaks.FirstOrDefaultAsync();
            if (streak == null)
            {
                streak = new Streak
                {
                    CurrentStreak = 1,
                    LongestStreak = 1,
                    LastEntryDate = entryDate.Date,
                    UpdatedAt = DateTime.Now
                };
                _context.Streaks.Add(streak);
            }
            else
            {
                if (streak.LastEntryDate.HasValue)
                {
                    var daysDifference = (entryDate.Date - streak.LastEntryDate.Value.Date).Days;

                    if (daysDifference == 1)
                    {
             
                        streak.CurrentStreak++;
                        if (streak.CurrentStreak > streak.LongestStreak)
                        {
                            streak.LongestStreak = streak.CurrentStreak;
                        }
                    }
                    else if (daysDifference > 1)
                    {
           
                        streak.CurrentStreak = 1;
                    }
              
                }
                else
                {
                    streak.CurrentStreak = 1;
                }

                streak.LastEntryDate = entryDate.Date;
                streak.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }

        private async Task RecalculateStreaksAsync()
        {
            var allEntries = await _context.JournalEntries
                .OrderBy(e => e.Date)
                .Select(e => e.Date.Date)
                .Distinct()
                .ToListAsync();

            if (!allEntries.Any())
            {
                var streak = await _context.Streaks.FirstOrDefaultAsync();
                if (streak != null)
                {
                    streak.CurrentStreak = 0;
                    streak.LastEntryDate = null;
                    await _context.SaveChangesAsync();
                }
                return;
            }

            int currentStreak = 1;
            int longestStreak = 1;

            for (int i = 1; i < allEntries.Count; i++)
            {
                if ((allEntries[i] - allEntries[i - 1]).Days == 1)
                {
                    currentStreak++;
                    longestStreak = Math.Max(longestStreak, currentStreak);
                }
                else
                {
                    currentStreak = 1;
                }
            }

            var streakRecord = await _context.Streaks.FirstOrDefaultAsync();
            if (streakRecord == null)
            {
                streakRecord = new Streak();
                _context.Streaks.Add(streakRecord);
            }

            streakRecord.CurrentStreak = currentStreak;
            streakRecord.LongestStreak = longestStreak;
            streakRecord.LastEntryDate = allEntries.Last();
            streakRecord.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<Streak> GetStreakAsync()
        {
            var streak = await _context.Streaks.FirstOrDefaultAsync();
            if (streak == null)
            {
                streak = new Streak
                {
                    CurrentStreak = 0,
                    LongestStreak = 0,
                    UpdatedAt = DateTime.Now
                };
                _context.Streaks.Add(streak);
                await _context.SaveChangesAsync();
            }

            if (streak.LastEntryDate.HasValue)
            {
                var daysSinceLastEntry = (DateTime.Now.Date - streak.LastEntryDate.Value.Date).Days;
                if (daysSinceLastEntry > 1)
                {
                    streak.CurrentStreak = 0;
                }
            }

            return streak;
        }

        public async Task<List<DateTime>> GetMissedDaysAsync(DateTime startDate, DateTime endDate)
        {
            var entries = await _context.JournalEntries
                .Where(e => e.Date >= startDate && e.Date <= endDate)
                .Select(e => e.Date.Date)
                .ToListAsync();

            var missedDays = new List<DateTime>();
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (!entries.Contains(date))
                {
                    missedDays.Add(date);
                }
            }

            return missedDays;
        }

        public async Task<Dictionary<string, int>> GetMoodDistributionAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.JournalEntries.Include(e => e.PrimaryMood).AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(e => e.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.Date <= endDate.Value);
            }

            var entries = await query.ToListAsync();

            var distribution = new Dictionary<string, int>
            {
                { "Positive", 0 },
                { "Neutral", 0 },
                { "Negative", 0 }
            };

            foreach (var entry in entries)
            {
                if (entry.PrimaryMood != null)
                {
                    distribution[entry.PrimaryMood.Category]++;
                }
            }

            return distribution;
        }

        public async Task<string> GetMostFrequentMoodAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.JournalEntries.Include(e => e.PrimaryMood).AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(e => e.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.Date <= endDate.Value);
            }

            var moodCounts = await query
                .GroupBy(e => e.PrimaryMood.Name)
                .Select(g => new { Mood = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefaultAsync();

            return moodCounts?.Mood ?? "None";
        }

        public async Task<Dictionary<string, int>> GetMostUsedTagsAsync(DateTime? startDate = null, DateTime? endDate = null, int topN = 10)
        {
            var query = _context.JournalEntries.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(e => e.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.Date <= endDate.Value);
            }

            var entries = await query.Where(e => !string.IsNullOrEmpty(e.TagIds)).ToListAsync();

            var tagCounts = new Dictionary<int, int>();

            foreach (var entry in entries)
            {
                if (string.IsNullOrWhiteSpace(entry.TagIds)) continue;

                var tagIds = entry.TagIds.Split(',').Select(int.Parse);
                foreach (var tagId in tagIds)
                {
                    if (tagCounts.ContainsKey(tagId))
                    {
                        tagCounts[tagId]++;
                    }
                    else
                    {
                        tagCounts[tagId] = 1;
                    }
                }
            }

            var allTags = await _context.Tags.ToListAsync();
            var result = new Dictionary<string, int>();

            foreach (var kvp in tagCounts.OrderByDescending(x => x.Value).Take(topN))
            {
                var tag = allTags.FirstOrDefault(t => t.Id == kvp.Key);
                if (tag != null)
                {
                    result[tag.Name] = kvp.Value;
                }
            }

            return result;
        }

        public async Task<double> GetAverageWordCountAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.JournalEntries.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(e => e.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.Date <= endDate.Value);
            }

            var count = await query.CountAsync();
            if (count == 0) return 0;

            var totalWords = await query.SumAsync(e => e.WordCount);
            return (double)totalWords / count;
        }

        public async Task<List<(DateTime Date, int WordCount)>> GetWordCountTrendsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.JournalEntries.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(e => e.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.Date <= endDate.Value);
            }

            return await query
                .OrderBy(e => e.Date)
                .Select(e => new ValueTuple<DateTime, int>(e.Date, e.WordCount))
                .ToListAsync();
        }

        private int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            return text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
