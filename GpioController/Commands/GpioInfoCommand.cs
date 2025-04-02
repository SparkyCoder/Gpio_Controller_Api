using GpioController.Commands.Request;
using GpioController.Commands.Results;
using GpioController.Parsers;
using GpioController.Services;

namespace GpioController.Commands;

public class GpioInfoCommand(IParser<GpioInfoResult> parser, ITerminalService terminalService) : BashCommand<GpioInfoRequest, GpioInfoResult>(parser, terminalService)
{
    protected override Func<GpioInfoRequest, string> Command { get; } = _ => "gpioinfo";
}