using CarCare.Processing.Models.Dto;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// Data context for the My Cars view.
/// </summary>
public interface IMyCarsContext
{
    /// <summary>
    /// The cars registered by the user.
    /// </summary>
    ObservableCollection<CarDto> Cars { get; }

    /// <summary>
    /// The selected page, starting from 1.
    /// </summary>
    int Page { get; set; }

    /// <summary>
    /// The number of cars per page.
    /// </summary>
    int ItemsPerPage { get; set; }

    /// <summary>
    /// Shows a view to add a new car.
    /// </summary>
    ICommand AddNewCarCommand { get; }

    /// <summary>
    /// Goes to the next page.
    /// </summary>
    ICommand NextPageCommand { get; }

    /// <summary>
    /// Goes to the previous page.
    /// </summary>
    ICommand PreviousPageCommand { get; }

    /// <summary>
    /// Increases the number of items per page.
    /// </summary>
    ICommand IncreaseItemsPerPageCommand { get; }

    /// <summary>
    /// Decreases the number of items per page.
    /// </summary>
    ICommand DecreaseItemsPerPageCommand { get; }
}
