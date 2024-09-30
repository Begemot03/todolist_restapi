using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using todolist_api.Services;

namespace todolist_api.Filters
{
    public class AuthorizeTaskAttribute : TypeFilterAttribute
    {
        public AuthorizeTaskAttribute() : base(typeof(TaskAuthorizeFilter))
        {}

        private class TaskAuthorizeFilter : IAsyncActionFilter
        {
            private readonly IAuthorizeService _authorizeService;

            public TaskAuthorizeFilter(IAuthorizeService authorizeService)
            {
                _authorizeService = authorizeService;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var userId = int.Parse(context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var taskId = (int)context.ActionArguments["taskId"];

                var hasAccess = await _authorizeService.UserHasAccessToTaskAsync(taskId, userId);

                if(!hasAccess)
                {
                    context.Result = new ForbidResult();
                    return;
                }

                await next();
            }
        }
    }
}