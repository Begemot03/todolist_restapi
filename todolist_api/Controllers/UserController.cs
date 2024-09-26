using Microsoft.AspNetCore.Mvc;
using todolist_api.Models;
using todolist_api.Repositories;
using todolist_api.Services;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IBaseRepository<User> users) : ControllerBase
    {
        private IBaseRepository<User> Users { get; set; }  = users;

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            return new JsonResult(await Users.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            return new JsonResult(await Users.Get(id));
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewUser(User user)
        {
            if(user == null)
            {
                return BadRequest("Invalid user data");
            }

            try
            {
                await Users.Create(user);

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}