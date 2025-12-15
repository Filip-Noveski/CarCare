namespace CarCare.Api.MockApi.Models;

public class Request
{
    public string Endpoint { get; set; } = string.Empty;

    public Dictionary<string, string> QueryParameters { get; set; } = new();
}
