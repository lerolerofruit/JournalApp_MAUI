using JournalApp.Models;
using JournalApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JournalApp.Views
{
    public partial class EntryEditorPage : Page
    {
        private readonly JournalService _journalService;
        private JournalEntry _currentEntry;
        private List<Mood> _allMoods;
        private List<Tag> _allTags;
        private List<Category> _allCategories;

        public EntryEditorPage(JournalService journalService, JournalEntry entry = null)
        {
            InitializeComponent();
            _journalService = journalService;
            _currentEntry = entry;
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                _allMoods = await _journalService.GetAllMoodsAsync();
                _allTags = await _journalService.GetAllTagsAsync();
                _allCategories = await _journalService.GetAllCategoriesAsync();

                cmbPrimaryMood.ItemsSource = _allMoods;
                cmbSecondaryMood1.ItemsSource = _allMoods;
                cmbSecondaryMood2.ItemsSource = _allMoods;
                cmbCategory.ItemsSource = _allCategories;
                lstTags.ItemsSource = _allTags;

                if (_currentEntry != null)
                {
                    LoadEntry();
                }
                else
                {
                    dpDate.SelectedDate = DateTime.Today;
                    txtTitle.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadEntry()
        {
            dpDate.SelectedDate = _currentEntry.Date;
            txtTitle.Text = _currentEntry.Title;
            txtContent.Text = _currentEntry.Content;
            
            cmbPrimaryMood.SelectedItem = _allMoods.FirstOrDefault(m => m.Id == _currentEntry.PrimaryMoodId);
            if (_currentEntry.SecondaryMood1Id.HasValue)
                cmbSecondaryMood1.SelectedItem = _allMoods.FirstOrDefault(m => m.Id == _currentEntry.SecondaryMood1Id);
            if (_currentEntry.SecondaryMood2Id.HasValue)
                cmbSecondaryMood2.SelectedItem = _allMoods.FirstOrDefault(m => m.Id == _currentEntry.SecondaryMood2Id);
            
            if (_currentEntry.CategoryId.HasValue)
                cmbCategory.SelectedItem = _allCategories.FirstOrDefault(c => c.Id == _currentEntry.CategoryId);

            if (!string.IsNullOrEmpty(_currentEntry.TagIds))
            {
                var tagIds = _currentEntry.TagIds.Split(',').Select(int.Parse).ToList();
                foreach (Tag tag in lstTags.Items)
                {
                    if (tagIds.Contains(tag.Id))
                    {
                        lstTags.SelectedItems.Add(tag);
                    }
                }
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!dpDate.SelectedDate.HasValue)
                {
                    MessageBox.Show("Please select a date.", "Validation Error", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtContent.Text))
                {
                    MessageBox.Show("Please enter some content.", "Validation Error", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbPrimaryMood.SelectedItem == null)
                {
                    MessageBox.Show("Please select a primary mood.", "Validation Error", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var entry = _currentEntry ?? new JournalEntry();
                entry.Date = dpDate.SelectedDate.Value;
                entry.Title = txtTitle.Text;
                entry.Content = txtContent.Text;
                entry.PrimaryMoodId = ((Mood)cmbPrimaryMood.SelectedItem).Id;
                entry.SecondaryMood1Id = (cmbSecondaryMood1.SelectedItem as Mood)?.Id;
                entry.SecondaryMood2Id = (cmbSecondaryMood2.SelectedItem as Mood)?.Id;
                entry.CategoryId = (cmbCategory.SelectedItem as Category)?.Id;

                var selectedTags = lstTags.SelectedItems.Cast<Tag>().ToList();
                entry.TagIds = string.Join(",", selectedTags.Select(t => t.Id));

                if (_currentEntry == null)
                {
                    await _journalService.CreateEntryAsync(entry);
                    MessageBox.Show("Entry saved successfully!", "Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    await _journalService.UpdateEntryAsync(entry);
                    MessageBox.Show("Entry updated successfully!", "Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                NavigationService?.Navigate(new DashboardPage(_journalService));
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving entry: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEntry == null) return;

            var result = MessageBox.Show("Are you sure you want to delete this entry?", 
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _journalService.DeleteEntryAsync(_currentEntry.Id);
                    MessageBox.Show("Entry deleted successfully!", "Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService?.Navigate(new DashboardPage(_journalService));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting entry: {ex.Message}", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private async void BtnAddCustomTag_Click(object sender, RoutedEventArgs e)
        {
            var tagName = txtCustomTag.Text?.Trim();
            if (string.IsNullOrWhiteSpace(tagName))
            {
                MessageBox.Show("Please enter a tag name.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newTag = await _journalService.CreateCustomTagAsync(tagName);
                _allTags = await _journalService.GetAllTagsAsync();
                lstTags.ItemsSource = _allTags;
                lstTags.SelectedItems.Add(newTag);
                txtCustomTag.Clear();
                MessageBox.Show("Custom tag created!", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating tag: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
