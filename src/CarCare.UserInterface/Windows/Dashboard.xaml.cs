using CarCare.UserInterface.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CarCare.UserInterface.Windows;

/// <summary>
/// Interaction logic for Dashboard.xaml
/// </summary>
public partial class Dashboard : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="Dashboard"/> class.
    /// </summary>
    public Dashboard(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        SessionControl sessionControl = serviceProvider.GetRequiredService<SessionControl>();
        MainGrid.Children.Add(sessionControl);
    }
}
