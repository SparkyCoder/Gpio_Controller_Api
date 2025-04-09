using GpioController.Models;

namespace GpioController.Commands.Results;

public class GpioInfoResult : Result
{
    public required IEnumerable<Gpio> Result { get; set; }
}