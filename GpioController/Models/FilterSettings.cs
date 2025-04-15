namespace GpioController.Models;

public class FilterSettings
{
    public List<int> AllowOnlyTheseChipsets { get; set; } = new();
    public List<int> AllowOnlyTheseGpios { get; set; } = new();
}