using Microsoft.AspNetCore.Authorization;

namespace GpioController.Authorization;

public class ConditionalAuthRequirement : IAuthorizationRequirement { }

public class ConditionalJwtAuthorization(IConfiguration config) : AuthorizationHandler<ConditionalAuthRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ConditionalAuthRequirement requirement)
    {
        var requireAuth = config.GetValue<bool>("Authorization:Enabled");

        if (!requireAuth || (context.User.Identity?.IsAuthenticated ?? false))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}



