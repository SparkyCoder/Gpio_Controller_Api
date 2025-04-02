using GpioController.Commands;
using GpioController.Commands.Results;

namespace GpioController.Factories;

public interface ICommandFactory
{
    ICommand<TRequest, TResult> GetCommand<TRequest, TResult>() where  TResult : Result;
}