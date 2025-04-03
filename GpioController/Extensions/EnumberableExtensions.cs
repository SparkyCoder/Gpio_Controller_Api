using GpioController.Models;

namespace GpioController.Extensions;

public static class EnumberableExtensions
{
    public static bool HasGpio(this IEnumerable<Gpio> gpios, int id)
    {
        return gpios.Any(gpio => gpio.Id == id);
    }
    
    public static IEnumerable<Gpio> Chipset(this IEnumerable<Gpio> gpios, int chipsetId)
    {
        return gpios.Where(gpio => gpio.Chipset == chipsetId);
    }
}