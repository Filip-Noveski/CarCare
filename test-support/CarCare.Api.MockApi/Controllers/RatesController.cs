using CarCare.Api.MockApi.Models;
using CarCare.Api.MockApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Api.MockApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RatesController : ControllerBase
{
    private readonly ILogger<RatesController> _logger;
    private readonly MockConfigService<Rate> _mockConfigService;
    private readonly RequestsService _requestsService;

    public RatesController(
        ILogger<RatesController> logger,
        MockConfigService<Rate> mockConfigService,
        RequestsService requestsService)
    {
        _logger = logger;
        _mockConfigService = mockConfigService;
        _requestsService = requestsService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Rate>> Get([FromQuery] string from, [FromQuery] string to)
    {
        _logger.LogInformation("Received GET request for rates from {From} to {To}", from, to);

        Request request = new()
        {
            Endpoint = HttpContext.Request.Path,
            QueryParameters = HttpContext.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString())
        };
        _requestsService.EnqueueRequest(request);

        MockConfig<Rate> config = _mockConfigService.DequeueResponse();
        return config.StatusCode switch
        {
            200 => Ok(config.Response),
            _ => StatusCode(config.StatusCode)
        };
    }
}
