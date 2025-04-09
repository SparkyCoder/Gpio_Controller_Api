using System.Security.Claims;
using GpioController.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GpioController.Controllers;

public class SecureController(IOptions<AuthorizationSettings> authorizationSettings) : ControllerBase
{
    private readonly AuthorizationSettings authorizationSettings = authorizationSettings.Value;

    protected bool IsAuthorized()
    {
        return !authorizationSettings.Enabled || IsEmailClaimAuthorized();
    }

    private bool IsEmailClaimAuthorized()
    {
        const string validationType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        
        var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
        
        var emailCliam =
            userIdentity?.Claims?.FirstOrDefault(claim => claim.Type == validationType);

        var currentUserEmail = emailCliam?.Value ?? string.Empty;
        
        var isAuthorized = authorizationSettings?.AuthorizedEmails?.Contains(currentUserEmail) ?? false;

        return isAuthorized;
    }
}