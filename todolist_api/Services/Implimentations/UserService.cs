using System.Security.Claims;

namespace todolist_api.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public UserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public int GetUserId()
        {
            var userIdClaim = _httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is no authenticated");

            if(!int.TryParse(userIdClaim, out int userId))
            {
                throw new FormatException();
            }

            return userId;
        }
    }
}