using GpioController.Commands;
using GpioController.Commands.Results;
using GpioController.Exceptions;

namespace GpioController.Factories;

public class CommandFactory(IServiceProvider provider) : ICommandFactory
{
    private static readonly Lock LockingMechanism = new();

    public ICommand<TRequest, TResult> GetCommand<TRequest, TResult>() where TResult : Result
    {
        lock (LockingMechanism)
        {
            var command = provider.GetService<ICommand<TRequest, TResult>>();

            if (command == null)
                throw new CommandNotFoundException(typeof(TResult));

            return command;
        }
    }
}
