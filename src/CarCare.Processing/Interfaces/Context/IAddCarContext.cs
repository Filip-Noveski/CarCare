using CarCare.Processing.Enums;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Interfaces.Context;

/// <summary>
/// Data context for the Add Car window.
/// </summary>
public interface IAddCarContext : IWindowContext
{
    /// <summary>
    /// The currencies that may be selected between.
    /// </summary>
    Currency[] AvailableCurrencies { get; }

    /// <summary>
    /// The manufacturer of the car.
    /// </summary>
    string Manufacturer { get; set; }

    /// <summary>
    /// The model of the car.
    /// </summary>
    string Model { get; set; }

    /// <summary>
    /// The specification of the car.
    /// </summary>
    string Specification { get; set; }

    /// <summary>
    /// The production year of the car.
    /// </summary>
    string ModelYear { get; set; }

    /// <summary>
    /// The error related to the Manufacturer.
    /// </summary>
    string ManufacturerError { get; set; }

    /// <summary>
    /// The error related to the Model.
    /// </summary>
    string ModelError { get; set; }

    /// <summary>
    /// The error related to the Specification
    /// </summary>
    string SpecificationError { get; set; }

    /// <summary>
    /// The error related to the ModelYear.
    /// </summary>
    string ModelYearError { get; set; }

    /// <summary>
    /// An optional cover image of the car.
    /// </summary>
    BitmapImage? MainImage { get; set; }

    /// <summary>
    /// The date the car was purchased.
    /// </summary>
    DateTime PurchaseDate { get; set; }

    /// <summary>
    /// The price the car was purchased for.
    /// </summary>
    string PurchasePrice { get; set; }

    /// <summary>
    /// An error related to the PurchasePrice.
    /// </summary>
    string PurchasePriceError { get; set; }

    /// <summary>
    /// The currency of the purchase.
    /// </summary>
    Currency PurchaseCurrency { get; set; }

    /// <summary>
    /// Adds a car with the provided data.
    /// </summary>
    ICommand AddCarCommand { get; }

    /// <summary>
    /// Opens a window to select a new main image.
    /// </summary>
    ICommand ChooseMainImageCommand { get; }

    /// <summary>
    /// Deletes the selected main image.
    /// </summary>
    ICommand DeleteMainImageCommand { get; }
}
