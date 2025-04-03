using GpioController.Commands.Request;
using GpioController.Models;

namespace GpioController.Services;

public interface IGpioService
{
    IEnumerable<Gpio> GetGpios();
    Gpio GetGpioById(int id);
    void UpdateState(IEnumerable<GpioSetRequest> updateRequests);
}