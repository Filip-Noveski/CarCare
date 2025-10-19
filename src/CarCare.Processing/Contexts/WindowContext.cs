using CarCare.Processing.Abstract;
using CarCare.Processing.Commands;
using CarCare.Processing.Interfaces.Context;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CarCare.Processing.Contexts;

internal class WindowContext : Context, IWindowContext
{
    public string MaximiseButtonChar
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public ICommand CloseCommand { get; }

    public ICommand ToggleMaximiseCommand { get; }

    public ICommand MinimiseCommand { get; }

    public WindowContext()
    {
        MaximiseButtonChar = "\U0001f5d7";  // assume we start maximised
        CloseCommand = new Command(Close);
        ToggleMaximiseCommand = new Command(ToggleMaximise);
        MinimiseCommand = new Command(Minimise);
    }

    private void Close(object? parameter)
    {
        if (parameter is not Window window)
        {
            return;
        }

        window.Close();
    }

    private void ToggleMaximise(object? parameter)
    {
        if (parameter is not Window window)
        {
            return;
        }

        (window.WindowState, MaximiseButtonChar) = window.WindowState switch
        {
            WindowState.Normal => (WindowState.Maximized, "\U0001f5d7"),
            WindowState.Maximized => (WindowState.Normal, "\U0001f5d6"),
            _ => throw new UnreachableException()
        };
    }

    private void Minimise(object? parameter)
    {
        if (parameter is not Window window)
        {
            return;
        }

        window.WindowState = WindowState.Minimized;
    }
}
