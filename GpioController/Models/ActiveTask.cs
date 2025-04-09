using GpioController.Commands.Request;

namespace GpioController.Models;

public class ActiveTask
{
    public CancellationTokenSource TokenSource { get; set; }
    public GpioSetRequest ActiveRequest { get; set; }
}