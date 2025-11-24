namespace CarCare.Persistence.Models;

/// <summary>
/// A database car model.
/// </summary>
public class CarDao
{
    /// <summary>
    /// The id of the car.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The id of the user that owns the car.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The manufacturer of the car.
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// The model of the car.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// The specification (trim/engine etc.) of the car.
    /// </summary>
    public string Specification { get; set; }

    /// <summary>
    /// The year the car was built.
    /// </summary>
    public int ModelYear { get; set; }

    /// <summary>
    /// The main UI image of the car.
    /// </summary>
    public byte[]? Image { get; set; }

    /// <summary>
    /// Creates a new instance of the <see cref="CarDao"/> class.
    /// </summary>
    public CarDao()
    {
        Id = Guid.Empty;
        UserId = Guid.Empty;
        Manufacturer = string.Empty;
        Model = string.Empty;
        Specification = string.Empty;
        ModelYear = -1;
        Image = null;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CarDao"/> class.
    /// </summary>
    /// <param name="id">The car's id.</param>
    /// <param name="userId">The owner's id.</param>
    /// <param name="manufacturer">The manufacturer name.</param>
    /// <param name="model">The model name.</param>
    /// <param name="spec">The specification name.</param>
    /// <param name="modelYear">The year of production.</param>
    public CarDao(Guid id, Guid userId, string manufacturer, string model, string spec, int modelYear)
    {
        Id = id;
        UserId = userId;
        Manufacturer = manufacturer;
        Model = model;
        Specification = spec;
        ModelYear = modelYear;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CarDao"/> class.
    /// </summary>
    /// <param name="id">The car's id.</param>
    /// <param name="userId">The owner's id.</param>
    /// <param name="manufacturer">The manufacturer name.</param>
    /// <param name="model">The model name.</param>
    /// <param name="spec">The specification name.</param>
    /// <param name="modelYear">The year of production.</param>
    /// <param name="image">The main image.</param>
    public CarDao(
        Guid id, Guid userId, string manufacturer, string model, string spec, int modelYear, byte[]? image) 
        : this(id, userId, manufacturer, model, spec, modelYear)
    {
        Image = image;
    }
}
