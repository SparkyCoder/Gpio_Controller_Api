using GpioController.Commands.Request;
using GpioController.Models;

namespace GpioController.Services;

public interface IGpioService
{
    IEnumerable<Gpio> GetGpios();
    Gpio GetGpioById(int chipsetId, int gpioId);
}