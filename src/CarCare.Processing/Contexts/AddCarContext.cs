using CarCare.Processing.Commands;
using CarCare.Processing.Enums;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Core;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CarCare.Processing.Contexts;

internal class AddCarContext : WindowContext, IAddCarContext
{
    private readonly ICarService _carService;
    private readonly IUserSession _session;
    private readonly IEventManagerService _eventManager;
    private readonly IFileDialogueService _fileService;

    public Currency[] AvailableCurrencies => Enum.GetValues<Currency>();

    public string Manufacturer
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }
    
    public string Model
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public string Specification
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public string ModelYear 
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public string ManufacturerError
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }
    
    public string ModelError
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public string SpecificationError
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public string ModelYearError
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }
    
    public BitmapImage? MainImage 
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public DateTime PurchaseDate
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public string PurchasePrice
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public string PurchasePriceError
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public Currency PurchaseCurrency { get; set; }

    public ICommand AddCarCommand { get; }

    public ICommand ChooseMainImageCommand { get; }

    public ICommand DeleteMainImageCommand { get; }

    public AddCarContext(
        ICarService carService,
        IUserSession session,
        IEventManagerService eventManager,
        IFileDialogueService fileService)
    {
        _carService = carService;
        _session = session;
        _eventManager = eventManager;
        _fileService = fileService;

        Manufacturer = string.Empty;
        Model = string.Empty;
        Specification = string.Empty;
        ModelYear = DateTime.Now.Year.ToString();
        PurchaseDate = DateTime.Now;
        PurchasePrice = "1500";
        PurchaseCurrency = Currency.Eur;

        ManufacturerError = string.Empty;
        ModelError = string.Empty;
        SpecificationError = string.Empty;
        ModelYearError = string.Empty;
        PurchasePriceError = string.Empty;

        AddCarCommand = new AsyncCommand(AddCar);
        ChooseMainImageCommand = new Command(ChooseMainImage);
        DeleteMainImageCommand = new Command(DeleteMainImage);
    }

    private async Task AddCar(object? parameter)
    {
        if (parameter is not Window window)
        {
            return;
        }

        bool error = false;
        if (string.IsNullOrWhiteSpace(Manufacturer))
        {
            ManufacturerError = "Please specify a manufacturer";
            error = true;
        }
        if (string.IsNullOrWhiteSpace(Model))
        {
            ModelError = "Please specify a model";
            error = true;
        }
        if (!int.TryParse(ModelYear, out int modelYearInt))
        {
            ModelYearError = "Please enter an integer for the model year";
            error = true;
        }
        if (modelYearInt > DateTime.Now.Year + 1 || modelYearInt < 1880)
        {
            ModelYearError = "Please specify a valid model year";
            error = true;
        }
        if (!double.TryParse(PurchasePrice, out double purchasePriceDouble) || purchasePriceDouble < 0)
        {
            PurchasePriceError = "Please enter a valid purchase price";
            error = true;
        }

        if (error)
        {
            return;
        }

        CarLifecycle lifecycle = new(
            new(PurchaseDate.Year, PurchaseDate.Month, PurchaseDate.Day),
            purchasePriceDouble,
            PurchaseCurrency);
        Car car = new(
            Guid.NewGuid(),
            _session.User!.Id,
            Manufacturer,
            Model,
            Specification,
            modelYearInt,
            MainImage,
            lifecycle);
        await _carService.AddAsync(car);
        _eventManager.OnMyCarsChanged();
        CloseCommand.Execute(window);
    }

    private void ChooseMainImage(object? parameter)
    {
        MainImage = _fileService.GetImageFile();
    }

    private void DeleteMainImage(object? parameter)
    {
        MainImage = null;
    }
}
