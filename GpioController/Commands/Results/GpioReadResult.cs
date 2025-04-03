namespace GpioController.Commands.Results;

public class GpioReadResult : Result
{
    public required string State { get; set; }
}