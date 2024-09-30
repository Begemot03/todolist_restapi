using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using todolist_api.Services;

namespace todolist_api.Filters
{
    public class AuthorizeBoardAttribute : TypeFilterAttribute
    {
        public AuthorizeBoardAttribute() : base(typeof(BoardAuthorizeFilter))
        {}

        private class BoardAuthorizeFilter : IAsyncActionFilter
        {
            private readonly IAuthorizeService _authorizeService;
            public BoardAuthorizeFilter(IAuthorizeService authorizeService) 
            {
                _authorizeService = authorizeService;
            }
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var userId = int.Parse(context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var boardId = (int)context.ActionArguments["boardId"];

                var hasAccess = await _authorizeService.UserHasAccessToBoardAsync(boardId, userId);

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