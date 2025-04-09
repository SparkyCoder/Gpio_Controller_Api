namespace GpioController.Models;

public class AuthorizationSettings
{
    public required bool Enabled { get; set; }
    public required List<string> AuthorizedEmails { get; set; }
}