using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Parsers;
using GpioController.Services;

namespace GpioController.Commands;

public class GpioReadCommand(IParser<GpioReadResult> parser, ITerminalService terminalService) : BashCommand<GpioReadRequest, GpioReadResult>(parser, terminalService)
{
    protected override Func<GpioReadRequest, string> Command { get; } = (request) => $"gpioget {request.Chipset} {request.Gpio}";
}