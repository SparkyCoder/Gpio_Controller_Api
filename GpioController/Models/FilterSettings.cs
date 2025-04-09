namespace GpioController.Models;

public class FilterSettings
{
    public FilterSettings()
    {
        AllowOnlyTheseChipsets = new();
        AllowOnlyTheseGpios = new();
    }

    public List<int> AllowOnlyTheseChipsets { get; set; }
    public List<int> AllowOnlyTheseGpios { get; set; }
}