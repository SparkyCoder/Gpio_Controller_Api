using System.Text.RegularExpressions;
using GpioController.Commands.Results;
using GpioController.Models;

namespace GpioController.Parsers;

public partial class ReadParser : IParser<GpioReadResult>
{
    public GpioReadResult Parse(string terminalOutput)
    {
        var cleanOutput = RemoveNonDigits().Replace(terminalOutput, "");
        return new GpioReadResult
        {
            State = State.Parse(cleanOutput).ToString()
        };
    }

    [GeneratedRegex(@"\D")]
    private static partial Regex RemoveNonDigits();
}