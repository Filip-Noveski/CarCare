using CarCare.Processing.Interfaces.Context;
using System.Windows.Controls;

namespace CarCare.UserInterface.Components;

/// <summary>
/// Interaction logic for Register.xaml
/// </summary>
public partial class Register : UserControl
{
    private readonly IRegisterContext _context;

    public Register(IRegisterContext context)
    {
        InitializeComponent();
        DataContext = context;
        _context = context;
    }
}
