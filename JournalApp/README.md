# Personal Journal Application - C# WPF

## Overview
A comprehensive desktop journaling application built with C# WPF and Entity Framework Core. This application provides a secure, feature-rich platform for daily journaling with mood tracking, analytics, and PDF export capabilities.

## Features

### ✅ Core Features (All 12 Requirements Implemented)

1. **Journal Entry Management**
   - Create, update, and delete daily entries
   - One entry per day limit
   - System-generated timestamps (CreatedAt, UpdatedAt)

2. **Rich Text/Markdown Writing**
   - Multi-line text editor for journal content
   - Support for long-form writing
   - Word count tracking

3. **Mood Tracking**
   - Primary mood (required): Happy, Excited, Relaxed, Grateful, Confident, Calm, Thoughtful, Curious, Nostalgic, Bored, Sad, Angry, Stressed, Lonely, Anxious
   - Up to 2 secondary moods (optional)
   - Categorized as Positive, Neutral, or Negative

4. **Tagging System**
   - 31 pre-built tags (Work, Career, Studies, Family, Friends, Health, Fitness, etc.)
   - Create custom tags
   - Multiple tag selection per entry

5. **Calendar Navigation**
   - Calendar view to navigate entries by date
   - Quick access to any day's entry

6. **Paginated Journal View**
   - Timeline view with pagination
   - 10 entries per page
   - Previous/Next navigation

7. **Search & Filter**
   - Search by title or content
   - Filter by date range
   - Filter by mood(s)
   - Filter by tags

8. **Streak Tracking**
   - Current daily streak
   - Longest streak achieved
   - Missed days calculation

9. **Theme Customization**
   - Light theme (default)
   - Dark theme
   - Theme preference saved per user

10. **Dashboard Analytics & Insights**
    - Mood distribution (Positive/Neutral/Negative)
    - Most frequent mood
    - Most used tags (top 10)
    - Average word count
    - Word count trends over time
    - All analytics filterable by date range

11. **Security & Privacy**
    - Password/PIN protected accounts
    - Secure password hashing with salt
    - User authentication system
    - Local data storage

12. **Export Journals**
    - Export entries to PDF by date range
    - Professional PDF formatting
    - Includes all entry details (mood, tags, category, timestamps)

## Technology Stack

- **Framework**: .NET 8.0 (Windows)
- **UI**: WPF (Windows Presentation Foundation)
- **Database**: SQLite with Entity Framework Core 8.0
- **PDF Export**: iText7 8.0.2
- **Architecture**: MVVM pattern with Services layer

## Project Structure

```
JournalApp/
├── Data/
│   └── JournalContext.cs          # EF Core DbContext
├── Models/
│   ├── JournalEntry.cs            # Main entry model
│   ├── Mood.cs                    # Mood model
│   ├── Tag.cs                     # Tag model
│   ├── Category.cs                # Category model
│   ├── User.cs                    # User/authentication model
│   └── Streak.cs                  # Streak tracking model
├── Services/
│   ├── JournalService.cs          # Core business logic
│   ├── AuthenticationService.cs   # Authentication & security
│   └── PdfExportService.cs        # PDF generation
├── Views/
│   ├── DashboardPage.xaml(.cs)    # Dashboard view
│   ├── EntryEditorPage.xaml(.cs)  # Entry creation/editing
│   ├── CalendarPage.xaml(.cs)     # Calendar navigation
│   ├── TimelinePage.xaml(.cs)     # Paginated timeline
│   ├── SearchPage.xaml(.cs)       # Search & filter
│   ├── AnalyticsPage.xaml(.cs)    # Analytics & insights
│   ├── ExportPage.xaml(.cs)       # PDF export
│   └── SettingsPage.xaml(.cs)     # Settings & preferences
├── MainWindow.xaml(.cs)           # Main application window
├── LoginWindow.xaml(.cs)          # Login/registration
├── App.xaml(.cs)                  # Application entry point
└── JournalApp.csproj              # Project file
```

## Installation & Setup

### Prerequisites
- .NET 8.0 SDK or later
- Windows 10/11
- Visual Studio 2022 (recommended) or VS Code with C# extensions

### Steps to Run

1. **Clone or extract the project**
   ```bash
   cd JournalApp
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

### First Time Setup

1. On first launch, you'll see the login screen
2. Click "Create New Account"
3. Enter a username and password (minimum 4 characters)
4. Click "Create New Account" to register
5. Login with your credentials

## Database

- **Location**: `%LocalAppData%\JournalApp\journal.db`
- **Type**: SQLite
- **Migrations**: Automatically applied on startup
- **Pre-seeded Data**: 
  - 15 moods (5 positive, 5 neutral, 5 negative)
  - 31 pre-built tags
  - 5 default categories

## Usage Guide

### Creating an Entry

1. Navigate to "New Entry" from the sidebar
2. Select a date (defaults to today)
3. Enter a title (optional)
4. Write your journal content
5. Select a primary mood (required)
6. Optionally select up to 2 secondary moods
7. Choose a category (optional)
8. Select tags or create custom tags
9. Click "Save Entry"

**Note**: You can only create one entry per day. Attempting to create a second entry for the same day will show an error.

### Viewing Entries

- **Dashboard**: See recent entries and quick stats
- **Calendar**: Click any date to view/edit that day's entry
- **Timeline**: Browse all entries with pagination

### Searching & Filtering

1. Go to "Search" in the sidebar
2. Use the search box to find entries by title or content
3. Or use filters:
   - Select date range
   - Choose mood(s)
   - Select tag(s)
4. Click "Apply Filters"

### Viewing Analytics

1. Navigate to "Analytics"
2. Optionally select a date range
3. View:
   - Mood distribution charts
   - Most frequent mood
   - Top 10 used tags
   - Average word count
   - Word count trends

### Exporting to PDF

1. Go to "Export PDF"
2. Select start and end dates
3. Click "Export to PDF"
4. Choose save location
5. PDF will be generated with all entries in the date range

### Changing Settings

1. Navigate to "Settings"
2. Switch between Light/Dark theme
3. Change your password if needed

## Security Features

- **Password Hashing**: SHA256 with unique salt per user
- **Local Storage**: All data stored locally on your machine
- **No Cloud Sync**: Complete privacy and control over your data
- **Session Management**: Secure login/logout functionality

## Code Quality Features

### Separation of Concerns
- Models for data structure
- Services for business logic
- Views for UI presentation
- Clear layered architecture

### Error Handling
- Try-catch blocks in all async operations
- User-friendly error messages
- Input validation
- Graceful degradation

### Modularity
- Reusable services
- Dependency injection pattern
- Single Responsibility Principle
- Interface-based design

### Best Practices
- Async/await for database operations
- LINQ for data queries
- Entity Framework conventions
- WPF MVVM patterns

## Extensibility

The application is designed to be easily extensible:

- **Add new moods**: Update the seed data in `JournalContext.cs`
- **Add new tags**: Insert into database or create via UI
- **New analytics**: Extend `JournalService.cs`
- **Custom themes**: Add to `MainWindow` resources
- **Export formats**: Create new export services

## Known Limitations

- Rich text formatting is plain text only (not HTML/Markdown rendering)
- No image attachments in entries
- No cloud synchronization
- Single user per installation
- Windows-only (WPF limitation)

## Future Enhancements

Potential features for future versions:
- Cloud backup/sync
- Image attachments
- Voice notes
- Reminders for daily journaling
- Data import/export (JSON, CSV)
- Advanced charts and visualizations
- Mobile companion app
- Multi-language support

## Troubleshooting

### Database Issues
If you encounter database errors:
1. Delete the database file at `%LocalAppData%\JournalApp\journal.db`
2. Restart the application
3. Database will be recreated automatically

### Build Errors
Ensure all NuGet packages are restored:
```bash
dotnet restore
dotnet clean
dotnet build
```

### Runtime Errors
Check that .NET 8.0 runtime is installed:
```bash
dotnet --version
```

## License

This is a coursework project for CS6004 Application Development.

## Author

Student ID: 213129
Course: CS6004NI - Application Development
Institution: Islington College (London Met University)
Academic Year: 2025-2026

## Acknowledgments

- Entity Framework Core team
- iText PDF library
- WPF framework documentation
- Stack Overflow community
