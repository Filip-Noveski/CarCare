using CarCare.Processing.Interfaces.Context;
using System.Windows;

namespace CarCare.UserInterface.Windows;

/// <summary>
/// Interaction logic for AddCar.xaml
/// </summary>
public partial class AddCar : Window
{
    private readonly IAddCarContext _context;

    public AddCar(IAddCarContext context)
    {
        _context = context;
        DataContext = context;
        InitializeComponent();
    }
}
