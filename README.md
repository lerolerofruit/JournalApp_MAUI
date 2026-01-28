# JournalApp (Full Feature) – .NET MAUI + SQLite

## What you get
- PIN lock
- Daily journal entry (one per day) with timestamps
- Markdown editor + preview
- Mood tracking (primary + up to 2 secondary)
- Tags (prebuilt + custom)
- Timeline (filter + pagination)
- Dashboard (streaks + summaries)
- Theme toggle (Light/Dark)
- PDF export (date range)

## How to run
1. Install Visual Studio 2022 with **.NET MAUI workload**.
2. Open `JournalApp.sln`
3. Restore NuGet packages (Visual Studio will prompt)
4. Run target: **Windows (WinUI)** (recommended)

## Notes
- Database file is stored under: `FileSystem.AppDataDirectory` as `journalapp.db3`
- PDF exports are saved in the same AppDataDirectory and opened via system viewer
