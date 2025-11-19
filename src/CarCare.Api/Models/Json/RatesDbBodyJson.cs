namespace CarCare.Api.Models.Json;

internal class RatesDbBodyJson
{
    public RatesDbDataJson Data { get; set; } = null!;
}

internal class RatesDbDataJson
{
    public DateTime Date { get; set; }

    public string From { get; set; } = null!;

    public Dictionary<string, double> Rates { get; set; } = new();
}