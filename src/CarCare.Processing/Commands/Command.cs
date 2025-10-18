using System.Windows.Input;

namespace CarCare.Processing.Commands;

internal class Command : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Predicate<object?>? _canExecute;

    public Command(Action<object?> execute)
    {
        _execute = execute;
    }

    public Command(Action<object?> execute, Predicate<object?>? canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    public event EventHandler? CanExecuteChanged;

    public void Execute(object? parameter) => _execute(parameter);
}
