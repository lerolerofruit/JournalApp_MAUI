using JournalApp.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace JournalApp.Views
{
    public partial class TimelinePage : Page
    {
        private readonly JournalService _journalService;
        private int _currentPage = 1;
        private const int _pageSize = 10;

        public TimelinePage(JournalService journalService)
        {
            InitializeComponent();
            _journalService = journalService;
            LoadEntries();
        }

        private async void LoadEntries()
        {
            try
            {
                var entries = await _journalService.GetEntriesPaginatedAsync(_currentPage, _pageSize);
                lstEntries.ItemsSource = entries;
                
                var totalCount = await _journalService.GetTotalEntriesCountAsync();
                txtPageInfo.Text = $"Page {_currentPage} of {Math.Ceiling((double)totalCount / _pageSize)}";
                
                btnPrevious.IsEnabled = _currentPage > 1;
                btnNext.IsEnabled = _currentPage < Math.Ceiling((double)totalCount / _pageSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading entries: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LstEntries_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstEntries.SelectedItem != null)
            {
                var entry = (Models.JournalEntry)lstEntries.SelectedItem;
                NavigationService?.Navigate(new EntryEditorPage(_journalService, entry));
            }
        }

        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadEntries();
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _currentPage++;
            LoadEntries();
        }
    }
}
