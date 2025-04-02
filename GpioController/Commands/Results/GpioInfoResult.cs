using GpioController.Models;

namespace GpioController.Commands.Results;

public class GpioInfoResult : Result
{
    public IEnumerable<Gpio> Result { get; set; }
}