using CarCare.Processing.Interfaces.Context;
using CarCare.UserInterface.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Shell;

namespace CarCare.UserInterface.Windows;

/// <summary>
/// Interaction logic for Dashboard.xaml
/// </summary>
public partial class Dashboard : Window
{
    /// <summary>
    /// Creates a new instance of the <see cref="Dashboard"/> class.
    /// </summary>
    public Dashboard(IDashboardContext context, IServiceProvider serviceProvider)
    {
        DataContext = context;
        InitializeComponent();

        SessionControl sessionControl = serviceProvider.GetRequiredService<SessionControl>();
        WindowChrome.SetIsHitTestVisibleInChrome(sessionControl, true);
        SessionControlContainer.Children.Add(sessionControl);

        MyCars myCars = serviceProvider.GetRequiredService<MyCars>();
        MyCarsTab.Content = myCars;
    }

    private void WindowStateChanged(object sender, EventArgs e)
    {
        ContentGrid.Margin = WindowState switch
        {
            WindowState.Maximized => new(7),
            _ => new(0)
        };
    }
}
