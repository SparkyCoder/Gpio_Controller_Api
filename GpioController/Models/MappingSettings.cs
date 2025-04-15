namespace GpioController.Models;

public class MappingSettings
{
    public List<Map> GpioNames { get; set; } = new();
}

public class Map
{
    public int Id { get; set; }
    public required string Name { get; set; }
}