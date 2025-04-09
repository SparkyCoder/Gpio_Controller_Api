using GpioController.Commands.Request;

namespace GpioController.Models;

public class ActiveTask
{
    public required CancellationTokenSource TokenSource { get; set; }
    public required GpioSetRequest ActiveRequest { get; set; }
}