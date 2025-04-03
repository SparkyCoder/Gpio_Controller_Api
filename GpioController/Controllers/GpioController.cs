using GpioController.Models;
using GpioController.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GpioController.Controllers;

[ApiController]
[Route("sbc")]
public class GpioController(IOptions<AuthorizationSettings> authorizationSettings, IGpioService gpioService) : SecureController(authorizationSettings)
{
    [AllowAnonymous]
    [HttpGet]
    [Route("chipsets/gpios")]
    public IActionResult Get()
    {
        var gpios = gpioService.GetGpios();
        return Ok(gpios);
    }
    
    [AllowAnonymous]
    [HttpGet]
    [Route("chipsets/{chipsetId}/gpios/{gpioId}")]
    public IActionResult GetById([FromRoute] int chipsetId, [FromRoute] int gpioId)
    {
        var gpio = gpioService.GetGpioById(chipsetId, gpioId);
        return Ok(gpio);
    }
}