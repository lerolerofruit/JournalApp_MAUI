using JournalApp.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace JournalApp.Views
{
    public partial class CalendarPage : Page
    {
        private readonly JournalService _journalService;

        public CalendarPage(JournalService journalService)
        {
            InitializeComponent();
            _journalService = journalService;
            calendar.SelectedDate = DateTime.Today;
        }

        private async void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calendar.SelectedDate.HasValue)
            {
                var entry = await _journalService.GetEntryByDateAsync(calendar.SelectedDate.Value);
                NavigationService?.Navigate(new EntryEditorPage(_journalService, entry));
            }
        }
    }
}
