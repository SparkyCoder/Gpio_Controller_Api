using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Exceptions;
using GpioController.Extensions;
using GpioController.Factories;
using GpioController.Models;

namespace GpioController.Services;

public class StateService(ICommandFactory commandFactory, IGpioService gpioService) : IStateService
{
    public GpioReadResult GetStateByGpioId(int chipsetId, int gpioId)
    {
        gpioService.GetGpioById(chipsetId, gpioId);
        
        var command = commandFactory.GetCommand<GpioReadRequest, GpioReadResult>();
        var result = command.Execute(new GpioReadRequest
        {
            Chipset = chipsetId,
            Gpio = gpioId
        });
        
        return result;
    }
    
    public void UpdateSingleState(GpioSetRequest request)
    {
        gpioService.GetGpioById(request.Chipset, request.Gpio);
        
        var command = commandFactory.GetCommand<GpioSetRequest, GpioSetResult>();
        command.Execute(request);
    }

    public void UpdateMultipleStates(IEnumerable<GpioSetRequest> updateRequests)
    {
        ValidateUpdateRequests(updateRequests);
        
        new Action(() =>
        {
            foreach (var request in updateRequests)
            {
                var command = commandFactory.GetCommand<GpioSetRequest, GpioSetResult>();
                command.Execute(request);
            }
        }).StartOnBackgroundThread();
    }

    private void ValidateUpdateRequests(IEnumerable<GpioSetRequest> updateRequests)
    {
        var gpios = gpioService.GetGpios();

        foreach (var request in updateRequests)
        {
            ValidateRequest(gpios, request);
        }
    }

    private static void ValidateRequest(IEnumerable<Gpio> gpios, GpioSetRequest request)
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