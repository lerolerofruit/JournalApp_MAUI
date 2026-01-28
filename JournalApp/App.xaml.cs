using JournalApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Threading.Tasks;
using JournalApp.Data;
using JournalApp.Services;

namespace JournalApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var context = new JournalContext();
            context.Database.Migrate();

            EnsureAdminUserExists(context).Wait();

            var loginWindow = new LoginWindow(context);
            loginWindow.Show();
        }

        private async Task EnsureAdminUserExists(JournalContext context)
        {
            var authService = new AuthenticationService(context);

            if (!await authService.HasAnyUsersAsync())
            {
                await authService.RegisterAsync("admin", "admin");
            }
        }
    }
}
