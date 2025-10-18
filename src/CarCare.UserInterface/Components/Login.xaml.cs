using CarCare.Processing.Interfaces.Context;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CarCare.UserInterface.Components;

/// <summary>
/// Interaction logic for Login.xaml
/// </summary>
public partial class Login : UserControl
{
    private const double PanelWidth = 400;
    private static readonly TimeSpan TransformAnimationTime = TimeSpan.FromMilliseconds(500);

    private readonly ILoginContext _context;

    public Login(ILoginContext context)
    {
        InitializeComponent();
        _context = context;
        DataContext = context;
        _context.UsernameChanged += AlterCredentialsPanel;
    }

    private async void Initialise(object sender, RoutedEventArgs e)
    {
        await _context.InitialiseAsync();
    }

    private void AlterCredentialsPanel(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_context.Username))
        {
            HideCredentialsPanel();
            return;
        }

        ShowCredentialsPanel();
    }

    private void ShowCredentialsPanel()
    {
        DoubleAnimation accountsAnimation = new()
        {
            To = -PanelWidth,
            Duration = TransformAnimationTime
        };
        DoubleAnimation credentialsAnimation = new()
        {
            To = 0,
            Duration = TransformAnimationTime
        };

        AccountsTransform.BeginAnimation(TranslateTransform.XProperty, accountsAnimation);
        CredentialsTransform.BeginAnimation(TranslateTransform.XProperty, credentialsAnimation);
    }

    private void HideCredentialsPanel()
    {
        DoubleAnimation accountsAnimation = new()
        {
            To = 0,
            Duration = TransformAnimationTime
        };
        DoubleAnimation credentialsAnimation = new()
        {
            To = PanelWidth,
            Duration = TransformAnimationTime
        };

        AccountsTransform.BeginAnimation(TranslateTransform.XProperty, accountsAnimation);
        CredentialsTransform.BeginAnimation(TranslateTransform.XProperty, credentialsAnimation);
    }
}
