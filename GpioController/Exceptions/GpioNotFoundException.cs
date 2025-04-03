namespace GpioController.Exceptions;

public class GpioNotFoundException(int chipsetId, int gpioId) : Exception($"Gpio id {gpioId} was not found on chipset {chipsetId}. Please check your SBC's documentation or get a full list from the /gpios endpoint.") { }