using CarCare.Processing.Abstract;
using CarCare.Processing.Commands;
using CarCare.Processing.Interfaces.Context;
using CarCare.Processing.Interfaces.Service;
using CarCare.Processing.Interfaces.Session;
using CarCare.Processing.Models.Core;
using CarCare.Processing.Models.Dto;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CarCare.Processing.Contexts;

internal class MyCarsContext : Context, IMyCarsContext
{
    private readonly ICarService _carService;
    private readonly IUserSession _session;
    private readonly IBitmapCreatorService _bitmapService;
    private readonly INavigationService _navigationService;
    private readonly IEventManagerService _eventManager;

    public ObservableCollection<CarDto> Cars { get; }

    public int Page
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public int ItemsPerPage
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddNewCarCommand { get; }

    public ICommand NextPageCommand { get; }

    public ICommand PreviousPageCommand { get; }

    public ICommand IncreaseItemsPerPageCommand { get; }

    public ICommand DecreaseItemsPerPageCommand { get; }

    public MyCarsContext(
        ICarService carService,
        IUserSession session,
        IBitmapCreatorService bitmapService,
        INavigationService navigationService,
        IEventManagerService eventManager)
    {
        _carService = carService;
        _session = session;
        _bitmapService = bitmapService;
        _navigationService = navigationService;
        _eventManager = eventManager;
        Cars = new();
        Page = 1;
        ItemsPerPage = 10;
        AddNewCarCommand = new Command(AddNewCar);
        NextPageCommand = new AsyncCommand(NextPage);
        PreviousPageCommand = new AsyncCommand(PreviousPage);
        IncreaseItemsPerPageCommand = new AsyncCommand(IncreaseItemsPerPage);
        DecreaseItemsPerPageCommand = new AsyncCommand(DecreaseItemsPerPage);
        Task.Run(LoadPage);

        _eventManager.MyCarsChanged += MyCarsChangedHandler;
        Task.Run(LoadPage);
    }

    ~MyCarsContext()
    {
        _eventManager.MyCarsChanged -= MyCarsChangedHandler;
    }

    private async void MyCarsChangedHandler(object? sender, EventArgs e) => await LoadPage();

    private void AddNewCar(object? parameter)
    {
        _navigationService.NavigateTo<IAddCarContext>();
    }

    private async Task LoadPage()
    {
        IEnumerable<Car> cars = await _carService.GetAllByUserIdAsync(_session.User!.Id, Page, ItemsPerPage);
        Application.Current.Dispatcher.Invoke(() =>
        {
            Cars.Clear();
            foreach (Car car in cars)
            {
                Cars.Add(car.ToDto());
            }
        });
    }

    private async Task NextPage(object? parameter)
    {
        Page++;
        await LoadPage();
    }

    private async Task PreviousPage(object? parameter)
    {
        Page--;
        if (Page < 1)
        {
            Page = 1;
        }

        await LoadPage();
    }

    private async Task IncreaseItemsPerPage(object? parameter)
    {
        ItemsPerPage = ItemsPerPage switch
        {
            < 1 => 1,
            >= 1 and < 5 => ItemsPerPage + 2,
            >= 5 and < 20 => ItemsPerPage + 5,
            >= 20 and < 50 => ItemsPerPage + 10,
            >= 50 => 50
        };

        await LoadPage();
    }

    private async Task DecreaseItemsPerPage(object? parameter)
    {
        ItemsPerPage = ItemsPerPage switch
        {
            <= 1 => 1,
            > 1 and <= 5 => ItemsPerPage - 2,
            > 5 and <= 20 => ItemsPerPage - 5,
            > 20 and <= 50 => ItemsPerPage - 10,
            > 50 => 50
        };

        await LoadPage();
    }
}
