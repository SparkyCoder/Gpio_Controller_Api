namespace GpioController.Extensions;

public static class StringExtensions
{
    public static string RemoveQuotes(this string input)
    {
        return input.Replace("\"", "");
    }
}