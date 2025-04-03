using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Models;
using GpioController.Parsers;
using GpioController.Services;

namespace GpioController.Commands;

public class GpioSetCommand(IParser<GpioSetResult> parser, ITerminalService terminalService) : BashCommand<GpioSetRequest, GpioSetResult>(parser, terminalService)
{
    private readonly ITerminalService terminalService = terminalService;

    protected override Func<GpioSetRequest, string> Command { get; } = request =>
    {
        request.State = State.Parse(request.State).ToString();
        return $"gpioset {request.Chipset} {request.Gpio}={request.State}";
    };

    protected override void RunOptionalPostCommandLogic(GpioSetRequest request, GpioSetResult result)
    {
        if (request.Options == null) return;

        var sleepTime = request.Options?.Milliseconds ?? 0;

        for (var timesRepeated = 0; timesRepeated < request.Options?.RepeatTimes; timesRepeated++)
        {
            Thread.Sleep(sleepTime);
            RunOpposite(request);
        }

        Thread.Sleep(sleepTime);
        RunOpposite(request);
    }

    private void RunOpposite(GpioSetRequest request)
    {
        request.State = State.ParseOpposite(request.State).ToString();
        var newCommand = Command(request);
        terminalService.RunCommand(newCommand);
    }
}