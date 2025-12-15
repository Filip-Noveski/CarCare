using CarCare.Api.MockApi.Models;
using CarCare.Api.MockApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Api.MockApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestsController : ControllerBase
{
    private readonly ILogger<RequestsController> _logger;
    private readonly RequestsService _requestsService;

    public RequestsController(ILogger<RequestsController> logger, RequestsService requestsService)
    {
        _logger = logger;
        _requestsService = requestsService;
    }

    [HttpGet]
    public ActionResult<Request> Get()
    {
        _logger.LogInformation("Received GET request for mock request");
        var request = _requestsService.DequeueRequest();
        return Ok(request);
    }
}
