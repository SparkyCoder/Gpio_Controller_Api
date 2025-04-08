namespace GpioController.Models;

public class FilterSettings
{
    public required List<int> AllowOnlyTheseChipsets { get; set; }
    public required List<int> AllowOnlyTheseGpios { get; set; }
}