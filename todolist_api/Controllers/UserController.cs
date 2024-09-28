using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todolist_api.Database;
using todolist_api.Models;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController(ApplicationContext context) : ControllerBase
    {
        private readonly ApplicationContext Context = context;

        [HttpGet("me")]
        public async Task<ActionResult> GetUser()
        {
            var username = HttpContext.User.FindFirstValue(ClaimTypes.UserData);

            if(string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            try
            {
                var user = await Context.Users.SingleOrDefaultAsync(user => username == user.Username);

                if(user == null)
                {
                    return NotFound("User not found");
                }


                return Ok(new { User = new UserGetDto()
                {
                    Id = user.Id,
                    Username = user.Username
                }});
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }


            
        }
    }
}