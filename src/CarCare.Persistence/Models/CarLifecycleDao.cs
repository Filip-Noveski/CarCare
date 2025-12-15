namespace CarCare.Persistence.Models;

/// <summary>
/// A database car lifecycle model.
/// </summary>
public class CarLifecycleDao
{
    /// <summary>
    /// The id of the car.
    /// </summary>
    public Guid CarId { get; set; }

    /// <summary>
    /// The date the car was bought.
    /// </summary>
    public DateOnly PurchaseDate { get; set; }

    /// <summary>
    /// The price the car was bought for.
    /// </summary>
    public double PurchasePrice { get; set; }

    /// <summary>
    /// The currency of the purchase price.
    /// </summary>
    public string PurchaseCurrency { get; set; }

    /// <summary>
    /// The date the car was sold.
    /// </summary>
    public DateOnly? SellDate { get; set; }

    /// <summary>
    /// The price the car was sold for.
    /// </summary>
    public double? SellPrice { get; set; }

    /// <summary>
    /// The currency of the sale price.
    /// </summary>
    public string? SellCurrency { get; set; }

    /// <summary>
    /// Creates a new <see cref="CarLifecycleDao"/> instance.
    /// </summary>
    public CarLifecycleDao()
    {
        CarId = Guid.Empty;
        PurchaseDate = DateOnly.MinValue;
        PurchasePrice = -1;
        PurchaseCurrency = string.Empty;
        SellDate = null;
        SellPrice = null;
        SellCurrency = null;
    }

    /// <summary>
    /// Creates a new <see cref="CarLifecycleDao"/> instance.
    /// </summary>
    /// <param name="carId">The id of the car.</param>
    /// <param name="purchaseDate">The purchase date.</param>
    /// <param name="purchasePrice">The purchase price.</param>
    /// <param name="purchaseCurrency">The purchase currency.</param>
    /// <param name="sellDate">The sale date.</param>
    /// <param name="sellPrice">The sale price.</param>
    /// <param name="sellCurrency">The sale currency.</param>
    public CarLifecycleDao(
        Guid carId,
        DateOnly purchaseDate,
        double purchasePrice,
        string purchaseCurrency,
        DateOnly? sellDate = null,
        double? sellPrice = null,
        string? sellCurrency = null)
    {
        CarId = carId;
        PurchaseDate = purchaseDate;
        PurchasePrice = purchasePrice;
        PurchaseCurrency = purchaseCurrency;
        SellDate = sellDate;
        SellPrice = sellPrice;
        SellCurrency = sellCurrency;
    }
}
