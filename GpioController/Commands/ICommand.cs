

namespace GpioController.Commands;

public interface ICommand<in TRequest, out TResult>
{
    TResult Execute(TRequest request);
}