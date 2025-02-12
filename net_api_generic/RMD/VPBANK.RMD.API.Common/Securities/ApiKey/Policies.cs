using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace VPBANK.RMD.API.Common.Securities.ApiKey
{
    public static class Policies
    {
        public const string OnlyAdministrators = nameof(OnlyAdministrators);
        public const string OnlyModerators = nameof(OnlyModerators);
        public const string OnlyUsers = nameof(OnlyUsers);
    }

    public static class Roles
    {
        public const string Administrator = "Administrator";
        public const string Moderator = "Moderator";
        public const string User = "User";
    }

    public class OnlyAdministratorsRequirement : IAuthorizationRequirement { }

    public class OnlyModeratorsRequirement : IAuthorizationRequirement { }

    public class OnlyUsersRequirement : IAuthorizationRequirement { }

    public class OnlyAdministratorsAuthorizationHandler : AuthorizationHandler<OnlyAdministratorsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyAdministratorsRequirement requirement)
        {
            if (context.User.IsInRole(Roles.Administrator))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class ModeratorsAuthorizationHandler : AuthorizationHandler<OnlyModeratorsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyModeratorsRequirement requirement)
        {
            if (context.User.IsInRole(Roles.Moderator))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class OnlyUsersAuthorizationHandler : AuthorizationHandler<OnlyUsersRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyUsersRequirement requirement)
        {
            if (context.User.IsInRole(Roles.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
