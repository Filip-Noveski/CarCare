using CarCare.Persistence.Models;
using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Models.Dto;
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

    public CarLifecycle Lifecycle { get; set; }

    public Car(Guid id,
        Guid userId,
        string manufacturer,
        string model,
        string specification,
        int modelYear,
        CarLifecycle lifecycle)
    {
        Id = id;
        UserId = userId;
        Manufacturer = manufacturer;
        Model = model;
        Specification = specification;
        ModelYear = modelYear;
        Lifecycle = lifecycle;
    }

    public Car(
        Guid id,
        Guid userId,
        string manufacturer,
        string model,
        string specification,
        int modelYear,
        BitmapImage? image,
        CarLifecycle lifecycle) 
        : this(id, userId, manufacturer, model, specification, modelYear, lifecycle)
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

    public CarDto ToDto()
    {
        CarLifecycleDto lifecycle = new(
            Lifecycle.PurchaseDate,
            Lifecycle.PurchasePrice,
            Lifecycle.PurchaseCurrency,
            Lifecycle.SaleDate,
            Lifecycle.SalePrice,
            Lifecycle.SaleCurrency);

        return new(
            Id,
            UserId,
            Manufacturer,
            Model,
            Specification,
            ModelYear,
            Image,
            lifecycle);
    }

    public CarLifecycleDao LifecycleToDao()
    {
        return new(
            Id,
            Lifecycle.PurchaseDate,
            Lifecycle.PurchasePrice,
            Lifecycle.PurchaseCurrency.ToString().ToUpper(),
            Lifecycle.SaleDate,
            Lifecycle.SalePrice,
            Lifecycle.SaleCurrency?.ToString().ToUpper());
    }
}

internal record CarLifecycle(
    DateOnly PurchaseDate,
    double PurchasePrice,
    Currency PurchaseCurrency,
    DateOnly? SaleDate = null,
    double? SalePrice = null,
    Currency? SaleCurrency = null);