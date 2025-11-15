using CarCare.Processing.Interfaces.Context;
using System.Windows.Controls;

namespace CarCare.UserInterface.Components;

/// <summary>
/// Interaction logic for AccountSettings.xaml
/// </summary>
public partial class AccountSettings : UserControl
{
    private readonly IAccountSettingsContext _context;

    public AccountSettings(IAccountSettingsContext context)
    {
        _context = context;
        DataContext = _context;
        InitializeComponent();
    }
}
