using JournalApp.Data;
using JournalApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace JournalApp
{
    public partial class LoginWindow : Window
    {
        private readonly JournalContext _context;

        public LoginWindow(JournalContext context)
        {
            InitializeComponent();
            _context = new JournalContext();
            
            // Ensure database is created
            _context.Database.Migrate();
            _context = context;
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            txtError.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                ShowError("Please enter both username and password.");
                return;
            }

            var authService = new AuthenticationService(_context);
            var success = await authService.LoginAsync(txtUsername.Text, txtPassword.Password);

            if (success)
            {
                var journalService = new JournalService(_context);
                var pdfService = new PdfExportService();
                var mainWindow = new MainWindow(_context, journalService, authService, pdfService);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ShowError("Invalid username or password.");
            }
        }

        private async void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            txtError.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                ShowError("Please enter both username and password.");
                return;
            }

            if (txtPassword.Password.Length < 4)
            {
                ShowError("Password must be at least 4 characters long.");
                return;
            }

            var authService = new AuthenticationService(_context);
            var success = await authService.RegisterAsync(txtUsername.Text, txtPassword.Password);

            if (success)
            {
                MessageBox.Show("Account created successfully! Please login.", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                txtPassword.Clear();
            }
            else
            {
                ShowError("Username already exists. Please choose a different username.");
            }
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
