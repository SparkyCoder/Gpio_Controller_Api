namespace GpioController.Commands.Request;

public class GpioSetRequest : Request
{
    public required int Chipset { get; set; }
    public required IEnumerable<int> Gpios { get; set; }
    public required string  State { get; set; }
    public OptionalSettings? Options { get; set; }
    public CancellationToken CancellationToken { get; set; }
}

public class OptionalSettings
{
    public int Milliseconds { get; set; }
    public int RepeatTimes { get; set; } = 1;
}