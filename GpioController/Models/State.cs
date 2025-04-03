using GpioController.Exceptions;

namespace GpioController.Models;

public class State(string state)
{
    public static State High => new("1");
    public static State Low => new("0");

    public static bool CanParse(string value)
    {
        return string.Equals(value, "High", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(value, "1", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(value, "Low", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(value, "0", StringComparison.OrdinalIgnoreCase);
    }

    public static State Parse(string value)
    {
        if (string.Equals(value, "High", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "1", StringComparison.OrdinalIgnoreCase))
            return High;
        if (string.Equals(value, "Low", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "0", StringComparison.OrdinalIgnoreCase))
            return Low;

        throw new InvalidStateException(value);
    }

    public static State ParseOpposite(string value)
    {
        if (string.Equals(value, "Low", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "0", StringComparison.OrdinalIgnoreCase))
            return High;
        if (string.Equals(value, "High", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "1", StringComparison.OrdinalIgnoreCase))
            return Low;

        throw new InvalidStateException(value);
    }
    
    public override string ToString()
    {
        return state;
    }
}