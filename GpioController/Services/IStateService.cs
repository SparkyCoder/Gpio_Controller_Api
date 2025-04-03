using GpioController.Commands.Request;
using GpioController.Commands.Results;

namespace GpioController.Services;

public interface IStateService
{
    GpioReadResult GetStateByGpioId(int chipsetId, int gpioId);
    void UpdateSingleState(GpioSetRequest request);
    void UpdateMultipleStates(IEnumerable<GpioSetRequest> updateRequests);
}