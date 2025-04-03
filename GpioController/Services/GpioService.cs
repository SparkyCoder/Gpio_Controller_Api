using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Exceptions;
using GpioController.Extensions;
using GpioController.Factories;
using GpioController.Models;

namespace GpioController.Services;

public class GpioService(ICommandFactory commandFactory) : IGpioService
{
    public IEnumerable<Gpio> GetGpios()
    {
        var command = commandFactory.GetCommand<GpioInfoRequest, GpioInfoResult>();
        var gpios = command.Execute(new GpioInfoRequest());

        if (!gpios?.Result.Any() ?? false)
            throw new NoGpiosFoundException();
        
        return gpios.Result;
    }

    public Gpio GetGpioById(int chipsetId, int gpioId)
    {
        var gpios = GetGpios();
        var requestedGpio = gpios.FirstOrDefault(gpio => gpio.Id == gpioId && gpio.Chipset == chipsetId);

        if (requestedGpio == null)
            throw new GpioNotFoundException(chipsetId, gpioId);

        return requestedGpio;
    }
}