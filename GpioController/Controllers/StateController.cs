using GpioController.Commands.Request;
using GpioController.Models;
using GpioController.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GpioController.Controllers;

[Authorize]
[ApiController]
[Route("sbc")]
public class StateController(IOptions<AuthorizationSettings> authorizationSettings, IStateService stateService) : SecureController(authorizationSettings)
{
    [HttpGet]
    [Route("chipsets/{chipsetId}/gpios/{gpioId}/state")]
    public IActionResult GetById([FromRoute] int chipsetId, [FromRoute] int gpioId)
    {
        if (!IsAuthorized())
            return Unauthorized();
        
        var result = stateService.GetStateByGpioId(chipsetId, gpioId);
        return Ok(result);
    }
    
    [HttpPost]
    [Route("chipsets/{chipsetId}/gpios/{gpioId}/state/{state}")]
    public IActionResult UpdateSingleStateById([FromRoute] int chipsetId, [FromRoute] int gpioId, [FromRoute] string state)
    {
        if (!IsAuthorized())
            return Unauthorized();

        stateService.UpdateSingleState(new GpioSetRequest
        {
            Chipset = chipsetId,
            Gpios = [gpioId],
            State = state
        } );
        
        return NoContent();
    }
    
    [HttpPost]
    [Route("chipsets/gpios/state")]
    public IActionResult UpdateMultipleStatesByRequest([FromRoute] int chipsetId, [FromRoute] int gpioId, [FromBody] IEnumerable<GpioSetRequest> updateRequests)
    {
        if (!IsAuthorized())
            return Unauthorized();

        stateService.UpdateMultipleStates(updateRequests);
        
        return NoContent();
    }
}