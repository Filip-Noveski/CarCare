using CarCare.Api.MockApi.Models;
using CarCare.Api.MockApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Api.MockApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RatesMockConfigController : ControllerBase
{
    private readonly ILogger<RatesMockConfigController> _logger;
    private readonly MockConfigService<Rate> _mockConfigService;

    public RatesMockConfigController(
        ILogger<RatesMockConfigController> logger,
        MockConfigService<Rate> mockConfigService)
    {
        _logger = logger;
        _mockConfigService = mockConfigService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] Rate rate, [FromQuery] int status)
    {
        _logger.LogInformation("Adding mock rate response with status {Status}", status);
        _mockConfigService.EnqueueResponse(rate, status);
        return Ok();
    }
}