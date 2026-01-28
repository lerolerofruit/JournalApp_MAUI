using JournalApp.Services;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace JournalApp.Views
{
    public partial class ExportPage : Page
    {
        private readonly JournalService _journalService;
        private readonly PdfExportService _pdfService;

        public ExportPage(JournalService journalService, PdfExportService pdfService)
        {
            InitializeComponent();
            _journalService = journalService;
            _pdfService = pdfService;
        }

        private async void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!dpStartDate.SelectedDate.HasValue || !dpEndDate.SelectedDate.HasValue)
                {
                    MessageBox.Show("Please select both start and end dates.", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var entries = await _journalService.FilterEntriesAsync(
                    dpStartDate.SelectedDate.Value,
                    dpEndDate.SelectedDate.Value);

                if (!entries.Any())
                {
                    MessageBox.Show("No entries found in the selected date range.", "No Data",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Load tags for each entry
                foreach (var entry in entries)
                {
                    if (!string.IsNullOrEmpty(entry.TagIds))
                    {
                        entry.Tags = await _journalService.GetTagsByIdsAsync(entry.TagIds);
                    }
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = $"Journal_{dpStartDate.SelectedDate:yyyyMMdd}_to_{dpEndDate.SelectedDate:yyyyMMdd}.pdf"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    _pdfService.ExportEntriesToPdf(entries, saveDialog.FileName);
                    MessageBox.Show($"Successfully exported {entries.Count} entries to PDF!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to PDF: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
