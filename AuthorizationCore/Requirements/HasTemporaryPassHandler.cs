using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AuthorizationCore.Requirments
{
    public class HasTemporaryPassHandler: AuthorizationHandler<OfficeEntryRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            OfficeEntryRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "TemporaryBadgeExpiry" &&
                                            c.Issuer == "https://contoso.com"))
            {
                return Task.CompletedTask;
            }

            var temporaryBadgeExpiry = 
                Convert.ToDateTime(context.User.FindFirst(
                    c => c.Type == "TemporaryBadgeExpiry" &&
                         c.Issuer == "https://contoso.com").Value);

            if (temporaryBadgeExpiry > DateTime.Now)
            {
                context.Succeed(requirement);
            }
            //context.Fail();
            return Task.CompletedTask;
        }
    }
}