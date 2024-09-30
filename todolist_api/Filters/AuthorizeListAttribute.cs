using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using todolist_api.Services;

namespace todolist_api.Filters
{
    public class AuthorizeListAttribute : TypeFilterAttribute
    {
        public AuthorizeListAttribute() : base(typeof(ListAuthorizeFilter))
        {}

        private class ListAuthorizeFilter : IAsyncActionFilter
        {
            private readonly IAuthorizeService _authorizeService;

            public ListAuthorizeFilter(IAuthorizeService authorizationService)
            {
                _authorizeService = authorizationService;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var userId = int.Parse(context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var listId = (int)context.ActionArguments["listId"];

                var hasAccess = await _authorizeService.UserHasAccessToListAsync(listId, userId);

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