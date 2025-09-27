using CarCare.Processing.Interfaces.Context;
using System.Windows;

namespace CarCare.UserInterface.Windows;

/// <summary>
/// Interaction logic for SplashScreen.xaml
/// </summary>
public partial class Splash : Window
{
    private readonly ISplashScreenContext _context;

    /// <summary>
    /// Creates a new instance of the <see cref="Splash"/> class.
    /// </summary>
    /// <param name="context">The data context for the window.</param>
    public Splash(ISplashScreenContext context)
    {
        _context = context;
        DataContext = context;
        InitializeComponent();
    }

    /// <summary>
    /// Runs the preparation tasks before launching the application.
    /// </summary>
    /// <returns>An awaitable <see cref="Task"/>.</returns>
    public async Task RunApplicationPreparationAsync()
    {
        await Task.Delay(250);  // mostly for testing
        await _context.InitialiseDatabaseAsync();
        await Task.Delay(500);  // mostly for testing
    }
}
