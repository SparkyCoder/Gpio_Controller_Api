namespace GpioController.Exceptions;

public class InvalidStateException(string value) : Exception($"The value {value} is not a valid state. Valid options are: High,  Low, 1, or 0.") { }