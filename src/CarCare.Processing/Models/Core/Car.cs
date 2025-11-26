using CarCare.Persistence.Models;
using CarCare.Processing.Interfaces.Service;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Models.Core;

internal class Car
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Manufacturer { get; set; }

    public string Model { get; set; }

    public string Specification { get; set; }

    public int ModelYear { get; set; }

    public BitmapImage? Image { get; set; }

    public Car(Guid id, Guid userId, string manufacturer, string model, string specification, int modelYear)
    {
        Id = id;
        UserId = userId;
        Manufacturer = manufacturer;
        Model = model;
        Specification = specification;
        ModelYear = modelYear;
    }

    public Car(
        Guid id,
        Guid userId,
        string manufacturer,
        string model,
        string specification,
        int modelYear,
        BitmapImage? image) 
        : this(id, userId, manufacturer, model, specification, modelYear)
    {
        Image = image;
    }

    public CarDao ToDao(IBitmapCreatorService bitmapCreator)
    {
        return new(
            Id,
            UserId,
            Manufacturer,
            Model,
            Specification,
            ModelYear,
            Image is null ? null : bitmapCreator.ConvertToBinary(Image));
    }
}
