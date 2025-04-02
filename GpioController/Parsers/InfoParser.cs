using System.Text.RegularExpressions;
using GpioController.Commands.Results;
using GpioController.Models;

namespace GpioController.Parsers;
    
public partial class InfoParser : IParser<GpioInfoResult>
{
    public GpioInfoResult Parse(string output)
    {
        var gpios = new List<Gpio>();

        var lines = GetIndividualLines(output);
        var gpio = new Gpio();

        foreach (var line in lines)
        {
            if (TryGetChipset(line, gpio))
                continue;
            
            if(TryGetGpioDetails(line, gpio))
                AddToList(gpios, gpio);
        }
        
        return new GpioInfoResult
        {
            Result =  gpios
        };
    }

    private static void AddToList(List<Gpio> gpios, Gpio gpio)
    {
        gpios.Add(new  Gpio
        {
            Id = gpio.Id,
            Chipset = gpio.Chipset,
            Name = gpio.Name,
        });
    }

    private IEnumerable<string> GetIndividualLines(string output)
    {
        return output.Split(["\n"], StringSplitOptions.RemoveEmptyEntries);
    }

    private static bool TryGetGpioDetails(string line, Gpio gpio)
    {
        var gpioPattern = GpioDetailsRegex();
        var gpioMatch = gpioPattern.Match(line);
    
        if (!gpioMatch.Success) 
            return false;
        
        gpio.Id = int.Parse(gpioMatch.Groups[1].Value);
        gpio.Name = (gpioMatch.Groups[2].Value);
        return true;
    }

    private static bool TryGetChipset(string line, Gpio gpio)
    {
        var chipPattern = ChipNumberRegex();
        var chipMatch = chipPattern.Match(line);

        if (!chipMatch.Success) return false;
        
        gpio.Chipset = int.Parse(chipMatch.Groups[1].Value);
        return true;

    }

    [GeneratedRegex(@"gpiochip(\d+)")]
    private static partial Regex ChipNumberRegex();

    [GeneratedRegex(@"\s+line\s+(\d+):\s+""([^""]*)""")]
    private static partial Regex GpioDetailsRegex();
}