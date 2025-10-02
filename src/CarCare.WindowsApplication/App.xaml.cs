using CarCare.Persistence.Extensions;
using CarCare.Processing.Extensions;
using CarCare.UserInterface.Extensions;
using CarCare.UserInterface.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace CarCare.WindowsApplication;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost AppHost { get; private set; } = null!;

    public App()
    {
        IHostBuilder builder = Host.CreateDefaultBuilder();
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile("appsettings.json");
#if DEBUG
            config.AddJsonFile("appsettings.Development.json");
#elif RELEASE
            config.AddJsonFile("appsettings.Production.json");
#endif
        });
        builder.ConfigureServices((context, services) =>
        {
            services.AddPersistenceServices();
            services.AddProcessingServices();
            services.AddUserInterfaceServices();
        });
        AppHost = builder.Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();

        Splash splash = AppHost.Services.GetRequiredService<Splash>();
        splash.Show();
        await splash.RunApplicationPreparationAsync();

        Authentication auth = AppHost.Services.GetRequiredService<Authentication>();
        splash.Close();
        auth.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost.StopAsync();
        AppHost.Dispose();

        base.OnExit(e);
    }
}
