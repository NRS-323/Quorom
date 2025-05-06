using Microsoft.AspNetCore.Authorization;
using Quorom;

namespace Umbono.Authorise
{
    public class OnlyQuoromAdminChecker : AuthorizationHandler<OnlyQuoromAdminChecker>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyQuoromAdminChecker requirement)
        {
           if(context.User.IsInRole(MyConstants.QuoromRoleNames.SuperUser))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
           return Task.CompletedTask;
        }
    }
}
