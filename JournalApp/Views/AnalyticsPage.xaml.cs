using JournalApp.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JournalApp.Views
{
    public partial class AnalyticsPage : Page
    {
        private readonly JournalService _journalService;

        public AnalyticsPage(JournalService journalService)
        {
            InitializeComponent();
            _journalService = journalService;
            LoadAnalytics();
        }

        private async void LoadAnalytics()
        {
            try
            {
                var startDate = dpStartDate.SelectedDate;
                var endDate = dpEndDate.SelectedDate;

                // Mood Distribution
                var moodDist = await _journalService.GetMoodDistributionAsync(startDate, endDate);
                txtPositive.Text = moodDist["Positive"].ToString();
                txtNeutral.Text = moodDist["Neutral"].ToString();
                txtNegative.Text = moodDist["Negative"].ToString();

                var total = moodDist.Values.Sum();
                if (total > 0)
                {
                    txtPositivePct.Text = $"{(moodDist["Positive"] * 100.0 / total):F1}%";
                    txtNeutralPct.Text = $"{(moodDist["Neutral"] * 100.0 / total):F1}%";
                    txtNegativePct.Text = $"{(moodDist["Negative"] * 100.0 / total):F1}%";
                }

                // Most Frequent Mood
                var freqMood = await _journalService.GetMostFrequentMoodAsync(startDate, endDate);
                txtFrequentMood.Text = freqMood;

                // Most Used Tags
                var tags = await _journalService.GetMostUsedTagsAsync(startDate, endDate, 10);
                lstTopTags.ItemsSource = tags.Select(kvp => $"{kvp.Key}: {kvp.Value}");

                // Average Word Count
                var avgWords = await _journalService.GetAverageWordCountAsync(startDate, endDate);
                txtAvgWords.Text = $"{avgWords:F0}";

                // Word Count Trends
                var trends = await _journalService.GetWordCountTrendsAsync(startDate, endDate);
                lstWordTrends.ItemsSource = trends.Select(t => $"{t.Item1:MMM dd}: {t.Item2} words");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading analytics: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadAnalytics();
        }
    }
}
