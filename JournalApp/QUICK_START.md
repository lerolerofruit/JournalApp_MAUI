# Quick Start Guide

## Get Up and Running in 5 Minutes

### Step 1: Prerequisites
Make sure you have:
- Windows 10 or 11
- .NET 8.0 SDK installed ([Download here](https://dotnet.microsoft.com/download/dotnet/8.0))

Check if .NET 8 is installed:
```bash
dotnet --version
```
Should show 8.0.x or higher.

### Step 2: Build the Project
Open a terminal in the JournalApp folder and run:

```bash
# Restore packages
dotnet restore

# Build the application
dotnet build

# Run the application
dotnet run
```

### Step 3: First Time Setup
1. The application will launch showing a login screen
2. Click **"Create New Account"**
3. Enter a username (e.g., "john")
4. Enter a password (minimum 4 characters, e.g., "1234")
5. Click **"Create New Account"** again
6. Login with your new credentials

### Step 4: Create Your First Entry
1. Click **"✏️ New Entry"** in the left sidebar
2. The date will be set to today automatically
3. Enter a title (optional): "My First Journal Entry"
4. Write something in the content area
5. Select a **Primary Mood** (required) - e.g., "Happy"
6. Optionally select secondary moods
7. Select some tags if you want
8. Click **"Save Entry"**

Congratulations! You've created your first journal entry! 🎉

### Step 5: Explore Features

#### View Dashboard
- Click **"📊 Dashboard"** to see your stats
- View total entries, current streak, and recent entries

#### Browse Timeline
- Click **"📝 Timeline"** to see all your entries
- Use Previous/Next buttons to navigate pages

#### Use Calendar
- Click **"📅 Calendar"** to view entries by date
- Click any date to view/edit that day's entry

#### Search Entries
- Click **"🔍 Search"** to find entries
- Type keywords to search
- Or use filters (date range, moods, tags)

#### View Analytics
- Click **"📈 Analytics"** to see insights
- View mood distribution
- See most used tags
- Check word count trends

#### Export to PDF
- Click **"📄 Export PDF"**
- Select a date range
- Click **"Export to PDF"**
- Choose where to save the file

#### Customize Theme
- Click **"⚙️ Settings"**
- Change theme to Dark or Light
- Update your password if needed

## Common Tasks

### Creating Multiple Entries
- You can only create ONE entry per day
- To write for a different day, select that date when creating
- To edit today's entry, just go to New Entry (it will load today's entry if it exists)

### Adding Custom Tags
1. When creating/editing an entry
2. Type your custom tag name in the "Custom Tag" box
3. Click "Add Custom Tag"
4. Select it from the tags list

### Tracking Your Streak
- Write an entry every day to build your streak
- Dashboard shows your current streak
- Try to beat your longest streak!

### Filtering by Mood
1. Go to Search page
2. Select one or more moods from the Moods list
3. Click "Apply Filters"
4. See all entries with those moods

## Tips & Tricks

### Productivity Tips
- 🔥 Write daily to build your streak
- 📝 Use tags to organize entries by topic
- 🎯 Set a daily reminder for yourself
- 📊 Check analytics monthly to see patterns

### Navigation Tips
- Double-click any entry in lists to edit it
- Use calendar for quick date jumping
- Dashboard provides quick actions

### Writing Tips
- Include details about your day
- Note your feelings and moods
- Use tags to track activities
- Add a title for easy identification

### Data Tips
- Your data is stored locally in `%LocalAppData%\JournalApp\`
- Export to PDF regularly for backups
- Database is SQLite (journal.db file)

## Troubleshooting

### Can't Create Entry
- **Error: "Entry already exists"**
  - You already have an entry for this date
  - Edit the existing entry instead
  - Or choose a different date

### Login Issues
- **Forgot Password?**
  - Unfortunately, password recovery isn't available
  - You'll need to delete the database and start fresh
  - Database location: `%LocalAppData%\JournalApp\journal.db`

### Application Won't Start
- Check .NET 8 is installed: `dotnet --version`
- Try rebuilding: `dotnet clean && dotnet build`
- Check for error messages in the terminal

### Export PDF Failed
- Make sure you selected both start and end dates
- Check you have write permission in the save location
- Verify there are entries in the selected date range

## Next Steps

Now that you're set up:
1. ✅ Create entries for the past week (backfill your journal)
2. ✅ Experiment with different moods and tags
3. ✅ Try switching to Dark theme
4. ✅ Build a 7-day writing streak
5. ✅ Export your first PDF

## Need Help?

- Check the full README.md for detailed documentation
- Review IMPLEMENTATION_NOTES.md for technical details
- All source code is commented for clarity

## Support

This is a coursework project. For questions:
- Review the code comments
- Check the implementation notes
- Refer to the marking scheme in the coursework document

---

**Happy Journaling! 📖✨**
