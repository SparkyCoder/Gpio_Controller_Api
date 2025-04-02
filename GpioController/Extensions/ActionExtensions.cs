namespace GpioController.Extensions;

public static class ActionExtensions
{
    public static void StartOnBackgroundThread(this Action action)
    {
        Task.Run(action);
    }
}