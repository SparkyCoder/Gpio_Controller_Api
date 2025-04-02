using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Extensions;
using GpioController.Factories;
using GpioController.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GpioController.Controllers;

[ApiController]
[Route("gpios")]
public class GpioController(IOptions<AuthorizationSettings> authorizationSettings, ICommandFactory commandFactory) : SecureController(authorizationSettings)
{
    [AllowAnonymous]
    [HttpGet(Name = "GetAll")]
    public IActionResult Get()
    {
        var command = commandFactory.GetCommand<GpioInfoRequest, GpioInfoResult>();
        var gpios = command.Execute(new GpioInfoRequest());
        return Ok(gpios);
    }
    
    [AllowAnonymous]
    // [Authorize]
    [HttpPatch(Name = "Patch")]
    public IActionResult Patch([FromBody] IEnumerable<GpioSetRequest> patchRequest)
    {
        // if (!IsAuthorized())
        //     return Unauthorized();

        new Action(() =>
        {
            foreach (var setRequest in patchRequest)
            {
                var command = commandFactory.GetCommand<GpioSetRequest, GpioSetResult>();
                command.Execute(setRequest);
            }
        }).StartOnBackgroundThread();
        
        return NoContent();
    }
}