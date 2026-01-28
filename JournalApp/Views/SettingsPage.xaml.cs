using JournalApp.Services;
using System.Windows;
using System.Windows.Controls;

namespace JournalApp.Views
{
    public partial class SettingsPage : Page
    {
        private readonly AuthenticationService _authService;
        private readonly MainWindow _mainWindow;

        public SettingsPage(AuthenticationService authService, MainWindow mainWindow)
        {
            InitializeComponent();
            _authService = authService;
            _mainWindow = mainWindow;
            
            cmbTheme.SelectedItem = _authService.CurrentUser?.Theme == "Dark" ? cmbDark : cmbLight;
        }

        private void CmbTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTheme.SelectedItem == null) return;

            var theme = ((ComboBoxItem)cmbTheme.SelectedItem).Content.ToString();
            _mainWindow.ApplyTheme(theme);
            _authService.UpdateThemeAsync(theme);
        }

        private async void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOldPassword.Password) || 
                string.IsNullOrWhiteSpace(txtNewPassword.Password))
            {
                MessageBox.Show("Please fill in both fields.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (txtNewPassword.Password.Length < 4)
            {
                MessageBox.Show("New password must be at least 4 characters.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var success = await _authService.ChangePasswordAsync(
                txtOldPassword.Password, txtNewPassword.Password);

            if (success)
            {
                MessageBox.Show("Password changed successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                txtOldPassword.Clear();
                txtNewPassword.Clear();
            }
            else
            {
                MessageBox.Show("Current password is incorrect.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
