namespace GpioController.Parsers;

public interface IParser<T>
{
    T Parse(string terminalOutput);
}