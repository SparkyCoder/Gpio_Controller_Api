using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Exceptions;
using GpioController.Factories;
using GpioController.Models;
using Microsoft.Extensions.Options;

namespace GpioController.Services;

public class GpioService(ICommandFactory commandFactory, IOptions<FilterSettings> filterSettings) : IGpioService
{
    public IEnumerable<Gpio> GetGpios()
    {
        var gpioFilter = filterSettings.Value;
        var command = commandFactory.GetCommand<GpioInfoRequest, GpioInfoResult>();
        var gpios = command.Execute(new GpioInfoRequest());
        var results = gpios?.Result ?? [];

        if (!gpios?.Result.Any() ?? false)
            throw new NoGpiosFoundException();

        if (results.Any() && gpioFilter.AllowOnlyTheseChipsets.Any())
            results = results.Where(gpio => gpioFilter.AllowOnlyTheseChipsets.Contains(gpio.Chipset));
        
        if (results.Any() && gpioFilter.AllowOnlyTheseGpios.Any())
            results = results.Where(gpio => gpioFilter.AllowOnlyTheseGpios.Contains(gpio.Id));
        
        return results;
    }

    public IEnumerable<Gpio?> OrderResultsByFilter(IEnumerable<Gpio> results)
    {
        var filter = filterSettings?.Value?.AllowOnlyTheseGpios ?? [];
        
        if (results.Any() && filter.Any())
            return filter
                .Select(id => results.FirstOrDefault(p => p.Id == id))
                .Where(p => p != null)
                .ToList();

        return results;
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