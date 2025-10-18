using CarCare.Processing.Interfaces.Context;
using System.Windows.Controls;

namespace CarCare.UserInterface.Components;

/// <summary>
/// Interaction logic for SessionControl.xaml
/// </summary>
public partial class SessionControl : UserControl
{
    private readonly ISessionControlContext _context;

    public SessionControl(ISessionControlContext context)
    {
        InitializeComponent();
        _context = context;
        DataContext = _context;
    }
}
