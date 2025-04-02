namespace GpioController.Exceptions;

public class CommandNotFoundException(Type type) : Exception($"Command of type {type} not found. Did you register your new command?") { }