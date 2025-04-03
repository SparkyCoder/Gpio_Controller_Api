namespace GpioController.Exceptions;

public class NoGpiosFoundException() : Exception("No Gpios found. Please verify API has permissions to run gpioinfo command.") { }