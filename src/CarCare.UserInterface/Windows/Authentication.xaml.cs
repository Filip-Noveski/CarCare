using CarCare.Processing.Interfaces.Context;
using CarCare.UserInterface.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CarCare.UserInterface.Windows;

/// <summary>
/// Interaction logic for Authentication.xaml
/// </summary>
public partial class Authentication : Window
{
    private const int WindowHeight = 275;
    private static readonly TimeSpan TransformAnimationTime = TimeSpan.FromMilliseconds(500);

    private readonly IAuthenticationContext _context;

    private readonly TranslateTransform _loginTransform = new()
    {
        Y = 0
    };
    private readonly TranslateTransform _registerTransform = new()
    {
        Y = WindowHeight
    };

    public Authentication(IAuthenticationContext context, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _context = context;
        DataContext = context;
        _context.LoginRequested += ShiftToShowLogin;
        _context.RegisterRequested += ShiftToShowRegister;
        Login loginComponent = serviceProvider.GetRequiredService<Login>();
        loginComponent.RenderTransform = _loginTransform;
        MainGrid.Children.Add(loginComponent);
        Register registerComponent = serviceProvider.GetRequiredService<Register>();
        registerComponent.RenderTransform = _registerTransform;
        MainGrid.Children.Add(registerComponent);
    }

    private void ShiftToShowRegister(object? sender, EventArgs e)
    {
        DoubleAnimation loginAnimation = new()
        {
            To = -WindowHeight,
            Duration = TransformAnimationTime
        };
        DoubleAnimation registerAnimation = new()
        {
            To = 0,
            Duration = TransformAnimationTime
        };

        _loginTransform.BeginAnimation(TranslateTransform.YProperty, loginAnimation);
        _registerTransform.BeginAnimation(TranslateTransform.YProperty, registerAnimation);
    }

    private void ShiftToShowLogin(object? sender, EventArgs e)
    {
        DoubleAnimation loginAnimation = new()
        {
            To = 0,
            Duration = TransformAnimationTime
        };
        DoubleAnimation registerAnimation = new()
        {
            To = WindowHeight,
            Duration = TransformAnimationTime
        };

        _loginTransform.BeginAnimation(TranslateTransform.YProperty, loginAnimation);
        _registerTransform.BeginAnimation(TranslateTransform.YProperty, registerAnimation);
    }
}
