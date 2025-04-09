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

        var individualCommands = request.Gpios.Select(gpio => $"gpioset {request.Chipset} {gpio}={request.State}").ToList();

        var terminalCommand = string.Join(';', individualCommands);

        return terminalCommand;
    };

    protected override void RunOptionalPostCommandLogic(GpioSetRequest request, GpioSetResult result)
    {
        if (request.Options == null) return;

        var sleepTime = request.Options?.Milliseconds ?? 0;

        for (var timesRepeated = 1; timesRepeated < request.Options?.RepeatTimes * 2; timesRepeated++)
        {
            Thread.Sleep(sleepTime);
            if (request.CancellationToken.IsCancellationRequested)
                break;
            RunOpposite(request);
        }
    }

    private void RunOpposite(GpioSetRequest request)
    {
        request.State = State.ParseOpposite(request.State).ToString();
        var newCommand = Command(request);
        terminalService.RunCommand(newCommand);
    }
}