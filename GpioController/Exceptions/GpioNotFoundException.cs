namespace GpioController.Exceptions;

public class GpioNotFoundException(int id) : Exception($"Gpio id {id} was not found. Please check your SBC's documentation or get a full list from the /gpios endpoint.") { }