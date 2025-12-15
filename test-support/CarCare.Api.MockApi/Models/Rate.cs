namespace CarCare.Api.MockApi.Models;

public class Rate
{
    public RateData? Data { get; set; }
}

public class RateData
{
    public string? Date { get; set; }

    public string? From { get; set; }

    public Dictionary<string, double>? Rates { get; set; }
}