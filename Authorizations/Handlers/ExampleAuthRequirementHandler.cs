using Microsoft.AspNetCore.Authorization;
using WPFIdentity.Authorizations.Requirements;


namespace WPFIdentity.Authorizations.Handlers
{
    public class ExampleAuthRequirementHandler : AuthorizationHandler<ExampleAuthRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExampleAuthRequirement requirement)
        {
            if(!context.User.HasClaim(c => c.Type == "ExampleClaim"))
                return Task.CompletedTask;

            var exampleDays = DateTime.Parse(context.User.FindFirst(c => c.Type == "ExampleClaim").Value);
            var diffDays = (DateTime.Now - exampleDays); 
            if(diffDays.Days > 30 * requirement.ProbationDays)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
