using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todolist_api.Database;
using todolist_api.Models;
using System.Security.Claims;
using todolist_api.Filters;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public BoardController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewBoard([FromBody]BoardDto boardDto)
        {
            if(boardDto == null || string.IsNullOrEmpty(boardDto.Title))
            {
                return BadRequest("Invalid board data");
            }

            var userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var user = await _context.Users.SingleAsync(user => user.Id == userId);

                var board = new Board()
                {
                    Title = boardDto.Title
                };

                _context.Boards.Add(board);
                user.Boards.Add(board);

                await _context.SaveChangesAsync();

                boardDto.Id = board.Id;

                return CreatedAtAction(nameof(GetBoard), new { board.Id }, boardDto);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpGet("{boardId}")]
        [AuthorizeBoard]
        public async Task<ActionResult> GetBoard(int boardId)
        {
            try
            {
                var board = await _context.Boards
                    .AsNoTracking()
                    .Where(b => b.Id == boardId)
                    .Select(b => new BoardDto()
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Lists = b.Lists.Select(l => new ListDto()
                        {
                            Id = l.Id,
                            Title = l.Title,
                            Tasks = l.Tasks.Select(t => new TaskDto()
                            {
                                Id = t.Id,
                                Title = t.Title
                            }).ToList()
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();
            

                if (board == null)
                {
                    return NotFound();
                }

                return Ok(board);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpDelete("{boardId}")]
        [AuthorizeBoard]
        public async Task<ActionResult> DeleteBoard(int boardId)
        {
            try
            {
                var board = await _context.Boards
                    .Where(b => b.Id == boardId)
                    .FirstOrDefaultAsync();

                if(board == null)
                {
                    return NotFound();
                }

                _context.Boards.Remove(board);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpPut("{boardId}")]
        [AuthorizeBoard]
        public async Task<ActionResult> UpdateBoard(int boardId, [FromBody] UpdateBoardDto updateBoardDto)
        {
            try
            {
                var board = await _context.Boards
                    .Where(b => b.Id == boardId)
                    .FirstOrDefaultAsync();

                if(board == null)
                {
                    return NotFound();
                }

                board.Title = updateBoardDto.Title;

                return Ok(updateBoardDto);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetBoards()
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var boards = await _context.Boards
                    .AsNoTracking()
                    .Where(b => b.Users.Any(u => u.Id == userId))
                    .Select(b => new BoardDto()
                    {
                        Id = b.Id,
                        Title = b.Title
                    })
                    .ToListAsync();

                if(boards == null)
                {
                    return NotFound();
                }

                return Ok(boards);
                    
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}