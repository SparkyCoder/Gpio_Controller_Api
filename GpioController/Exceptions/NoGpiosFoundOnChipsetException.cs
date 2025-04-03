namespace GpioController.Exceptions;

public class NoGpiosFoundOnChipsetException(int id, int chipset) : Exception($"Gpio {id} was not found on chipset {chipset}. Please verify your data and try again.")
{
    
}