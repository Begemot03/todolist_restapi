using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todolist_api.Database;
using todolist_api.Models;
using System.Security.Claims;
using todolist_api.Filters;
using todolist_api.Services;
using System.ComponentModel.DataAnnotations;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IUserService _userService;

        public BoardController(ApplicationContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewBoard([Required][FromBody]CreateBoardDto createBoardDto)
        {
            try
            {
                var userId = _userService.GetUserId();
                var user = await _context.Users.SingleAsync(user => user.Id == userId);

                var board = new Board()
                {
                    Title = createBoardDto.Title,
                    Lists = []
                };

                _context.Boards.Add(board);
                user.Boards.Add(board);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBoard), new { boardId = board.Id }, CreateBoardDto(board));
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
                var board = await GetBoardById(boardId);

                if (board == null)
                {
                    return NotFound();
                }

                return Ok(CreateBoardDto(board));
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
                var board = await GetBoardById(boardId);

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
        public async Task<ActionResult> UpdateBoard(int boardId, [Required][FromBody] UpdateBoardDto updateBoardDto)
        {
            try
            {
                var board = await GetBoardById(boardId);

                if(board == null)
                {
                    return NotFound();
                }

                board.Title = updateBoardDto.Title;
                
                await _context.SaveChangesAsync();

                return Ok(CreateBoardDto(board));
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
                var userId = _userService.GetUserId();

                var boards = await _context.Boards
                    .AsNoTracking()
                    .Where(b => b.Users.Any(u => u.Id == userId))
                    .Include(b => b.Lists)
                    .ThenInclude(l => l.Tasks)
                    .Select(b => CreateBoardDto(b))
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

        private async Task<Board?> GetBoardById(int boardId)
        {
            return await _context.Boards
                .Include(b => b.Lists)
                .ThenInclude(l => l.Tasks)
                .SingleOrDefaultAsync(b => b.Id == boardId);
        }

        private static BoardDto CreateBoardDto(Board board) => new()
        {
            Id = board.Id,
            Title = board.Title,
            Lists = board.Lists.Select(l => new ListDto()
            {
                Id = l.Id,
                Title = l.Title,
                Tasks = l.Tasks.Select(t => new TaskDto()
                {
                    Id = t.Id,
                    Title = t.Title
                }).ToList()
            }).ToList()
        };
    }
}