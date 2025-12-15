namespace CarCare.Api.Tests.TestModels;

internal class Request
{
    public string Endpoint { get; set; } = string.Empty;

    public Dictionary<string, string> QueryParameters { get; set; } = new();
}
