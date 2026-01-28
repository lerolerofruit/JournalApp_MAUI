using JournalApp.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace JournalApp.Views
{
    public partial class SearchPage : Page
    {
        private readonly JournalService _journalService;

        public SearchPage(JournalService journalService)
        {
            InitializeComponent();
            _journalService = journalService;
            LoadFilterData();
        }

        private async void LoadFilterData()
        {
            var moods = await _journalService.GetAllMoodsAsync();
            var tags = await _journalService.GetAllTagsAsync();
            
            lstMoodFilter.ItemsSource = moods;
            lstTagFilter.ItemsSource = tags;
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var entries = await _journalService.SearchEntriesAsync(txtSearch.Text);
                lstResults.ItemsSource = entries;
                txtResultCount.Text = $"{entries.Count} result(s) found";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var moodIds = new System.Collections.Generic.List<int>();
                foreach (Models.Mood mood in lstMoodFilter.SelectedItems)
                    moodIds.Add(mood.Id);

                var tagIds = new System.Collections.Generic.List<int>();
                foreach (Models.Tag tag in lstTagFilter.SelectedItems)
                    tagIds.Add(tag.Id);

                var entries = await _journalService.FilterEntriesAsync(
                    dpStartDate.SelectedDate,
                    dpEndDate.SelectedDate,
                    moodIds.Count > 0 ? moodIds : null,
                    tagIds.Count > 0 ? tagIds : null
                );

                lstResults.ItemsSource = entries;
                txtResultCount.Text = $"{entries.Count} result(s) found";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LstResults_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstResults.SelectedItem != null)
            {
                var entry = (Models.JournalEntry)lstResults.SelectedItem;
                NavigationService?.Navigate(new EntryEditorPage(_journalService, entry));
            }
        }
    }
}
