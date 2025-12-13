using CarCare.Processing.Interfaces.Context;
using System.Windows.Controls;

namespace CarCare.UserInterface.Components;

/// <summary>
/// Interaction logic for MyCars.xaml
/// </summary>
public partial class MyCars : UserControl
{
    private readonly IMyCarsContext _context;

    public MyCars(IMyCarsContext context)
    {
        _context = context;
        DataContext = context;
        InitializeComponent();
    }
}
