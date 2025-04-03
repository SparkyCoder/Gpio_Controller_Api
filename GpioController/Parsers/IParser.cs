namespace GpioController.Parsers;

public interface IParser<out TResult>
{
    TResult Parse(string terminalOutput);
}