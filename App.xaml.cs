using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using System.IO;
using System.Text;
using System.Windows;

using WPFIdentity.ViewModels;
using WPFIdentity.Authorizations.Requirements;
using WPFIdentity.Authorizations.Handlers;

namespace WPFIdentity
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow? _mainwindow;
        private ILogger? _logger;
        private IHost? _host;

        private void OnStartup(object sender, StartupEventArgs e)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            IConfigurationBuilder builder = new ConfigurationBuilder();
            BuildConfig(builder);

            _logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .CreateLogger();

            _logger.Information("App - OnStartup - Application Starting");
            _logger.Information("App - OnStartup - Adding Dependancies");

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Class Dependancies
                    services.AddSingleton(m => new MainWindow());
                    services.AddSingleton(_logger);
                    services.AddSingleton<IMainViewModel, MainViewModel>();

                    // Authorizations
                    services.AddRazorPages();
                    services.AddSingleton<IAuthorizationHandler, ExampleAuthRequirementHandler>();
                    services.AddAuthentication().AddCookie("MyCookieAuth",
                        options =>
                        {
                            options.Cookie.Name = "MyWFPCookieAuth";
                            //options.LoginPath = "/Account/Login";
                            //options.AccessDeniedPath = "/Account/AccessDenied";
                            options.ExpireTimeSpan = TimeSpan.FromDays(1);
                        });

                    services.AddAuthorization(options =>
                        {
                            options.AddPolicy("Employee", policy => policy.RequireClaim("Employee"));

                            options.AddPolicy("Admin", policy => policy
                            .RequireClaim("Employee")
                            .RequireClaim("Admin"));
                        });
                })
                

                // Normal
                .UseSerilog()
                .Build();
                

            _logger.Information("App - OnStartup - Creating the Main UI.");
            _mainwindow = _host.Services.GetRequiredService<MainWindow>();

            _logger.Information("App - OnStartup - Setting the UI DataContext.");
            _mainwindow.DataContext = ActivatorUtilities.CreateInstance<MainViewModel>(_host.Services);

            _logger.Information("App - OnStartup - Showing UI.");
            _mainwindow.Show();
        }

        private static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
