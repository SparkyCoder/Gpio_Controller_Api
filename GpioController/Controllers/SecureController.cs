using System.Security.Claims;
using GpioController.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GpioController.Controllers;

public class SecureController : ControllerBase
{
    private readonly AuthorizationSettings authorizationSettings;

    public SecureController(IOptions<AuthorizationSettings> authorizationSettings)
    {
        this.authorizationSettings = authorizationSettings.Value;
    }

    protected bool IsAuthorized()
    {
        var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
        var emailCliam =
            userIdentity?.Claims?.FirstOrDefault(claim => claim.Type == authorizationSettings?.ValidationType);

        var currentUserEmail = emailCliam?.Value ?? string.Empty;
        var isAuthorized = authorizationSettings?.AuthorizedEmails?.Contains(currentUserEmail) ?? false;

        return isAuthorized;
    }
}