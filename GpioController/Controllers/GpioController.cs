using GpioController.Commands.Request;
using GpioController.Models;
using GpioController.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GpioController.Controllers;

[ApiController]
[Route("gpios")]
public class GpioController(IOptions<AuthorizationSettings> authorizationSettings, IGpioService gpioService) : SecureController(authorizationSettings)
{
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Get()
    {
        var gpios = gpioService.GetGpios();
        return Ok(gpios);
    }
    
    [AllowAnonymous]
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var gpio = gpioService.GetGpioById(id);
        return Ok(gpio);
    }
    
    [AllowAnonymous]
    // [Authorize]
    [HttpPost]
    [Route("state")]
    public IActionResult GetStateById([FromBody] IEnumerable<GpioSetRequest> updateRequest)
    {
        // if (!IsAuthorized())
        //     return Unauthorized();

        gpioService.UpdateState(updateRequest);
        
        return NoContent();
    }
}