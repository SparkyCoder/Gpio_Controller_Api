namespace GpioController.Commands.Request;

public class GpioReadRequest
{
    public required int Chipset { get; set; }
    public required int  Gpio { get; set; }
}