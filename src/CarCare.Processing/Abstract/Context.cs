using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarCare.Processing.Abstract;

/// <summary>
/// Base class for Data Contexts of views.
/// </summary>
public abstract class Context : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Notifies that the property with the provided name has changed.
    /// </summary>
    /// <param name="propertyName">The name of the changed property.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
