using Microsoft.AspNetCore.Mvc;
using todolist_api.Models;
using todolist_api.Repositories;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController(IBaseRepository<Board> boards, IBaseRepository<User> users) : ControllerBase
    {
        public IBaseRepository<Board> Boards { get; set; } = boards;
        public IBaseRepository<User> Users { get; set; } = users;

        [HttpPost]
        public async Task<ActionResult> CreateNewBoard([FromQuery]int userId, [FromBody]Board board)
        {
            if(board == null)
            {
                return BadRequest("Invalid board data");
            }

            try
            {
                var user = await Users.Get(userId);

                board.Users.Add(user);
                user.Boards.Add(board);

                await Boards.Create(board);

                return CreatedAtAction(nameof(GetBoard), new { board.Id }, board);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
           
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBoard(int id)
        {
            return new JsonResult(await Boards.Get(id));
        }

    }
}