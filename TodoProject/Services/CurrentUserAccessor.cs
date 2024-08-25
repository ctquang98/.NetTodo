using System.Security.Claims;

namespace TodoProject.Services
{
    public class CurrentUserAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string? GetCurrentUserId()
        {
            return httpContextAccessor
                ?.HttpContext
                ?.User
                ?.Claims
                ?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                ?.Value;
        }

        public string? GetCurrentUserName()
        {
            return httpContextAccessor
                ?.HttpContext
                ?.User
                ?.Claims
                ?.FirstOrDefault(x => x.Type == ClaimTypes.Name)
                ?.Value;
        }
    }
}
