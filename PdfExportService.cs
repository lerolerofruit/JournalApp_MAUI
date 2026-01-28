using JournalApp.Models;
using System.Collections.Generic;

namespace JournalApp.Services
{
    public class PdfExportService
    {
        public void ExportToPdf(List<JournalEntry> entries, string filePath)
        {
            // Placeholder - implement PDF export using a library like iTextSharp or QuestPDF
            System.Windows.MessageBox.Show("PDF export feature coming soon!", "Info");
        }
    }
}