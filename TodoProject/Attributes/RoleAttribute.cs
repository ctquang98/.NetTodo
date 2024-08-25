using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoProject.Attributes
{
    public class RoleAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] roles;

        public RoleAttribute(params string[] roles)
        {
            this.roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var isUserHasRole = roles.Any(x => user.IsInRole(x));
            if (!isUserHasRole)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
