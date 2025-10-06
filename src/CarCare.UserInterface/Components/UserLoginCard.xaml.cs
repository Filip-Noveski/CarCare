using CarCare.Processing.Models.Dto;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarCare.UserInterface.Components;

/// <summary>
/// Interaction logic for UserCard.xaml
/// </summary>
public partial class UserLoginCard : UserControl
{
    public static readonly DependencyProperty SelectUserCommandProperty = DependencyProperty.Register(
        name: nameof(SelectUserCommand),
        propertyType: typeof(ICommand),
        ownerType: typeof(UserLoginCard));

    public ICommand SelectUserCommand
    {
        get => (ICommand)GetValue(SelectUserCommandProperty);
        set => SetValue(SelectUserCommandProperty, value);
    }

    public UserLoginCard()
    {
        InitializeComponent();
    }

    private void SelectUser(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is not UserDto user)
        {
            return;
        }

        SelectUserCommand.Execute(user.Username);
    }
}
