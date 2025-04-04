using GpioController.Commands.Request;
using GpioController.Models;
using GpioController.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GpioController.Controllers;

[ApiController]
[Route("sbc")]
public class StateController(IOptions<AuthorizationSettings> authorizationSettings, IStateService stateService) : SecureController(authorizationSettings)
{
    [AllowAnonymous]
    // [Authorize]
    [HttpGet]
    [Route("chipsets/{chipsetId}/gpios/{gpioId}/state")]
    public IActionResult GetById([FromRoute] int chipsetId, [FromRoute] int gpioId)
    {
        // if (!IsAuthorized())
        //     return Unauthorized();
        
        var result = stateService.GetStateByGpioId(chipsetId, gpioId);
        return Ok(result);
    }
    
    [AllowAnonymous]
    // [Authorize]
    [HttpPost]
    [Route("chipsets/{chipsetId}/gpios/{gpioId}/state/{state}")]
    public IActionResult UpdateSingleStateById([FromRoute] int chipsetId, [FromRoute] int gpioId, [FromRoute] string state)
    {
        // if (!IsAuthorized())
        //     return Unauthorized();

        stateService.UpdateSingleState(new GpioSetRequest
        {
            Chipset = chipsetId,
            Gpio = gpioId,
            State = state
        } );
        
        return NoContent();
    }
    
    [AllowAnonymous]
    // [Authorize]
    [HttpPost]
    [Route("chipsets/gpios/state")]
    public IActionResult UpdateMultipleStatesByRequest([FromRoute] int chipsetId, [FromRoute] int gpioId, [FromBody] IEnumerable<GpioSetRequest> updateRequests)
    {
        // if (!IsAuthorized())
        //     return Unauthorized();

        stateService.UpdateMultipleStates(updateRequests);
        
        return NoContent();
    }
}