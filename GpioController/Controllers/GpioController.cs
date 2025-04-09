using GpioController.Models;
using GpioController.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GpioController.Controllers;

[ApiController]
[Route("sbc")]
[Authorize(Policy = "ConditionalPolicy")]
public class GpioController(IOptions<AuthorizationSettings> authorizationSettings, IGpioService gpioService) : SecureController(authorizationSettings)
{
    [HttpGet]
    [Route("chipsets/gpios")]
    public IActionResult Get()
    {
        if (!IsAuthorized())
            return Unauthorized();
        
        var gpios = gpioService.GetGpios();
        var filteredResults = gpioService.OrderResultsByFilter(gpios);
        
        return Ok(filteredResults);
    }
    
    [HttpGet]
    [Route("chipsets/{chipsetId}/gpios/{gpioId}")]
    public IActionResult GetById([FromRoute] int chipsetId, [FromRoute] int gpioId)
    {
        if (!IsAuthorized())
            return Unauthorized();
        
        var gpio = gpioService.GetGpioById(chipsetId, gpioId);
        return Ok(gpio);
    }
}