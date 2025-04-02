using GpioController.Parsers;
using GpioController.Services;

namespace GpioController.Commands;

public abstract class BashCommand<TRequest, TResult>(IParser<TResult> parser, ITerminalService terminalService) : ICommand<TRequest, TResult>
{
    protected abstract Func<TRequest, string> Command { get; }

    protected virtual void RunOptionalPostCommandLogic(TRequest request, TResult result) { }

    public TResult Execute(TRequest request)
    {
        var command = Command(request);
        var terminalOutput = terminalService.RunCommand(command);
        var parsedResults = parser.Parse(terminalOutput);
        RunOptionalPostCommandLogic(request, parsedResults);
        return parsedResults;
    }
}