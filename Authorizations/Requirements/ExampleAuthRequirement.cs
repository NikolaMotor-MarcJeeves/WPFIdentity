using Microsoft.AspNetCore.Authorization;

namespace WPFIdentity.Authorizations.Requirements
{
    public class ExampleAuthRequirement : IAuthorizationRequirement
    {
        public int ProbationDays { get; }
        public ExampleAuthRequirement(int probationDays)
        {
            ProbationDays = probationDays;
        }
    }
}
