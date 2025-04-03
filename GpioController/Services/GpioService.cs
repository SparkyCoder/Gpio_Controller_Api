using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Exceptions;
using GpioController.Extensions;
using GpioController.Factories;
using GpioController.Models;

namespace GpioController.Services;

public class GpioService( ICommandFactory commandFactory) : IGpioService
{
    public IEnumerable<Gpio> GetGpios()
    {
        var command = commandFactory.GetCommand<GpioInfoRequest, GpioInfoResult>();
        var gpios = command.Execute(new GpioInfoRequest());

        if (!gpios?.Result.Any() ?? false)
            throw new NoGpiosFoundException();
        
        return gpios.Result;
    }

    public Gpio GetGpioById(int id)
    {
        var gpios = GetGpios();
        var requestedGpio = gpios.FirstOrDefault(gpio => gpio.Id == id);

        if (requestedGpio == null)
            throw new GpioNotFoundException(id);
        
        return requestedGpio;
    }

    public void UpdateState(IEnumerable<GpioSetRequest> updateRequests)
    {
        ValidateUpdateRequest(updateRequests);
        
        new Action(() =>
        {
            foreach (var request in updateRequests)
            {
                var command = commandFactory.GetCommand<GpioSetRequest, GpioSetResult>();
                command.Execute(request);
            }
        }).StartOnBackgroundThread();
    }

    private void ValidateUpdateRequest(IEnumerable<GpioSetRequest> updateRequests)
    {
        var gpios = GetGpios();

        foreach (var request in updateRequests)
        {
            var gpioId = request.Gpio;
            var chipsetId = request.Chipset;
            var state = request.State;
            
            if(!gpios.Chipset(chipsetId).HasGpio(gpioId))
                throw new NoGpiosFoundOnChipsetException(gpioId, chipsetId);
            
            if(!State.CanParse(state))
                throw new InvalidStateException(state);
        }
    }
}