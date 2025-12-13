using System.Windows.Input;

namespace CarCare.Processing.Commands;

internal class AsyncCommand : ICommand
{
    private readonly Func<object?, Task> _execute;
    private readonly Predicate<object?>? _canExecute;
    private bool _isExecuting;

    public AsyncCommand(Func<object?, Task> execute)
    {
        _execute = execute;
        _isExecuting = false;
    }

    public AsyncCommand(Func<object?, Task> execute, Predicate<object?>? canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
        _isExecuting = false;
    }

    public bool CanExecute(object? parameter)
    {
        return !_isExecuting && (_canExecute == null || _canExecute(parameter));
    }

    public event EventHandler? CanExecuteChanged;

    public async void Execute(object? parameter)
    {
        _isExecuting = true;
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        try
        {
            await _execute(parameter);
        }
        finally
        {
            _isExecuting = false;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
