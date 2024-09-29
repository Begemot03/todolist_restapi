using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todolist_api.Database;
using todolist_api.Models;
using System.Security.Claims;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BoardController(ApplicationContext context) : ControllerBase
    {
        private readonly ApplicationContext Context = context;

        [HttpPost]
        public async Task<ActionResult> CreateNewBoard([FromBody]BoardDto boardDto)
        {
            if(boardDto == null || string.IsNullOrEmpty(boardDto.Title))
            {
                return BadRequest("Invalid board data");
            }

            var username = HttpContext.User.FindFirstValue(ClaimTypes.UserData);

            try
            {
                var user = await Context.Users.SingleAsync(user => user.Username == username);

                var board = new Board()
                {
                    Title = boardDto.Title
                };

                Context.Boards.Add(board);
                user.Boards.Add(board);

                await Context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBoard), new { board.Id }, boardDto);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
           
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBoard(int id)
        {
            if(id < 0) return BadRequest("Invalid Id");

            var username = HttpContext.User.FindFirstValue(ClaimTypes.UserData);

            try
            {
                var hasAccess = await UserHasAccessToBoard(id, username);

                if(!hasAccess)
                {
                    return NotFound();
                }

                var board = await Context.Boards
                    .AsNoTracking()
                    .Where(b => b.Id == id)
                    .Select(b => new BoardDto()
                    {
                        Id = b.Id,
                        Title = b.Title
                    })
                    .FirstOrDefaultAsync();
            

                if (board == null)
                {
                    return NotFound();
                }

                return Ok(new { Board = board });
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        private async Task<bool> UserHasAccessToBoard(int boardId, string username)
        {
            return await Context.Boards
                .AsNoTracking()
                .AnyAsync(b => b.Id == boardId && b.Users.Any(u => u.Username == username));
        }

    }
}