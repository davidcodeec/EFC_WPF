using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using Infrastructure.Utils;
using System;
using Microsoft.Extensions.Configuration;

namespace Presentation_WPF_Employee
{
    public partial class App : Application
    {
        private static IHost? host;

        public App()
        {
            // Directly specify the connection string here
            var employeesconnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Exercises\\CSharp-Exercise\\EFC_WPF\\Infrastructure\\Data\\Employee_DB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True";

            var productsconnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Exercises\\CSharp-Exercise\\EFC_WPF\\Infrastructure\\Data\\ProductCatalog.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True";



            host = Host.CreateDefaultBuilder()

                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<EmployeeDbContext>(options =>
                    {
                        options.UseSqlServer(employeesconnectionString);
                    });

                    // Register the second DbContext with its connection string
                    services.AddDbContext<ProductCatalogDBContext>(options =>
                    {
                        options.UseSqlServer(productsconnectionString);
                    });

                    services.AddSingleton<ILogs>(provider =>
                    {
                        try
                        {
                            return new Logs(@"C:\Exercises\CSharp-Exercise\EFC_WPF\Infrastructure\Logs\", true);
                        }
                        catch (Exception ex)
                        {
                            // Log the initialization error using your custom logging mechanism
                            var logs = provider.GetRequiredService<ILogs>();
                            logs.LogWarningAsync($"LOGGER INITIALIZATION ERROR! {DateTime.Now} :: {ex.Message}", nameof(App));

                            // Return a fallback logger or handle the error appropriately
                            return new ConsoleLogger();
                        }
                    });

                    services.AddSingleton<MainWindow>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            host?.Start();

            var mainWindow = host?.Services.GetRequiredService<MainWindow>();
            mainWindow?.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            host?.StopAsync().Wait();
            host?.Dispose();

            base.OnExit(e);
        }
    }
}
