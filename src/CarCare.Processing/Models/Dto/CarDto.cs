using CarCare.Processing.Abstract;
using CarCare.Processing.Enums;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Models.Dto;

/// <summary>
/// A tranferable Car model.
/// </summary>
public class CarDto : Context
{
    /// <summary>
    /// The id of the car.
    /// </summary>
    public Guid Id
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// The id of the user that owns the car.
    /// </summary>
    public Guid UserId
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// The manufacturer.
    /// </summary>
    public string Manufacturer
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// The model.
    /// </summary>
    public string Model
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// The specification (engine, trim etc.).
    /// </summary>
    public string Specification
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// The year the car was built.
    /// </summary>
    public int ModelYear
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// The UI image of the car.
    /// </summary>
    public BitmapImage? Image
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// The car's lifecycle.
    /// </summary>
    public CarLifecycleDto Lifecycle { get; set; }

    /// <summary>
    /// Creates a new <see cref="CarDto"/> object.
    /// </summary>
    /// <param name="id">The id of the car.</param>
    /// <param name="userId">The owner's id.</param>
    /// <param name="manufacturer">The manufacturer.</param>
    /// <param name="model">The model.</param>
    /// <param name="specification">The specification.</param>
    /// <param name="modelYear">The model year.</param>
    public CarDto(
        Guid id,
        Guid userId,
        string manufacturer,
        string model,
        string specification,
        int modelYear,
        CarLifecycleDto lifecycle)
    {
        Id = id;
        UserId = userId;
        Manufacturer = manufacturer;
        Model = model;
        Specification = specification;
        ModelYear = modelYear;
        Lifecycle = lifecycle;
    }

    /// <summary>
    /// Creates a new <see cref="CarDto"/> object.
    /// </summary>
    /// <param name="id">The id of the car.</param>
    /// <param name="userId">The owner's id.</param>
    /// <param name="manufacturer">The manufacturer.</param>
    /// <param name="model">The model.</param>
    /// <param name="specification">The specification.</param>
    /// <param name="modelYear">The model year.</param>
    /// <param name="image">The UI image.</param>
    public CarDto(
        Guid id,
        Guid userId,
        string manufacturer,
        string model,
        string specification,
        int modelYear,
        BitmapImage? image,
        CarLifecycleDto lifecycle) : this(id, userId, manufacturer, model, specification, modelYear, lifecycle)
    {
        Image = image;
    }
}

/// <summary>
/// The lifecycle information of a car.
/// </summary>
/// <param name="PurchaseDate">The date the car was bought.</param>
/// <param name="PurchasePrice">The purchase price of the car.</param>
/// <param name="PurchaseCurrency">The currency of the purchase.</param>
/// <param name="SaleDate">The date the car was sold.</param>
/// <param name="SalePrice">The price the car was sold for.</param>
/// <param name="SaleCurrency">The currency of the sale.</param>
public record CarLifecycleDto(
    DateOnly PurchaseDate,
    double PurchasePrice,
    Currency PurchaseCurrency,
    DateOnly? SaleDate,
    double? SalePrice,
    Currency? SaleCurrency);