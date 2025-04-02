namespace GpioController.Models;

public class AuthorizationSettings
{
    public required string ValidationType { get; set; }
    public required List<string> AuthorizedEmails { get; set; }
}