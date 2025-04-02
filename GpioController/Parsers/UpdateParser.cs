using System.Text.RegularExpressions;
using GpioController.Commands.Results;
using GpioController.Models;

namespace GpioController.Parsers;
    
public class UpdateParser : IParser<GpioSetResult>
{
    public GpioSetResult Parse(string output)
    {
        return new GpioSetResult
        {
            Result = output
        };
    }
}