using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using TodoProject.Controllers;
using TodoProject.models;
using TodoProject.Services;

namespace TodoProject.Attributes
{
    public class CardLogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var user = context.HttpContext.User;
            var userId = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var userName = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var actionName = context.ActionDescriptor.DisplayName;
            var logger = context.HttpContext.RequestServices.GetService<ILogger<CardController>>();

            if (user.Identity.IsAuthenticated)
            {
                logger?.LogInformation(
                    "{userName} ({userId}): {actionName} at {time}",
                    userName,
                    userId,
                    actionName,
                    DateTime.Now.ToString()
                );
            }
            else
            {
                logger?.LogInformation("Anonymous: {actionName} at {time}", actionName, DateTime.Now.ToString());
            }

            base.OnActionExecuted(context);
        }
    }
}
