using CarCare.Processing.Interfaces.Context;
using CarCare.UserInterface.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CarCare.UserInterface.Windows;

/// <summary>
/// Interaction logic for AccountSettings.xaml
/// </summary>
public partial class Settings : Window
{
    private readonly ISettingsContext _context;

    public Settings(ISettingsContext context, IServiceProvider provider)
    {
        DataContext = context;
        _context = context;
        InitializeComponent();

        AccountSettings accSettings = provider.GetRequiredService<AccountSettings>();
        AccountSettingsTab.Content = accSettings;
    }
}
