using CarCare.Processing.Interfaces.Context;
using System.Windows.Controls;

namespace CarCare.UserInterface.Components;

/// <summary>
/// Interaction logic for ApplicationSettings.xaml
/// </summary>
public partial class ApplicationSettings : UserControl
{
    private readonly IApplicationSettingsContext _context;

    public ApplicationSettings(IApplicationSettingsContext context)
    {
        DataContext = context;
        _context = context;
        InitializeComponent();
    }
}
