using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todolist_api.Database;
using todolist_api.Filters;
using todolist_api.Models;
using todolist_api.Services;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IUserService _userService;

        public ListController(ApplicationContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet("{listId}")]
        [AuthorizeList]
        public async Task<ActionResult> GetList(int listId)
        {
            try
            {
                var list = await _context.Lists
                    .AsNoTracking()
                    .Include(l => l.Tasks)
                    .SingleOrDefaultAsync(list => list.Id == listId);

                if(list == null)
                {
                    return NotFound();
                }

                return Ok(CreateListDto(list));
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpDelete("{listId}")]
        [AuthorizeList]
        public async Task<ActionResult> DeleteList(int listId)
        {
            try
            {
                var list = await GetListById(listId);

                if(list == null)
                {
                    return NotFound();
                }

                _context.Lists.Remove(list);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpPut("{listId}")]
        [AuthorizeList]
        public async Task<ActionResult> UpdateList(int listId, [Required][FromBody] UpdateListDto updateListDto)
        {
            try
            {
                var list = await GetListById(listId);
                
                if(list == null)
                {
                    return NotFound();
                }

                list.Title = updateListDto.Title;
                
                await _context.SaveChangesAsync();

                return Ok(CreateListDto(list));
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewList([Required][FromBody]CreateListDto createListDto)
        {
            try
            {
                var userId = _userService.GetUserId();
                var board = await _context.Boards
                    .SingleOrDefaultAsync(b => b.Id == createListDto.BoardId && b.Users.Any(u =>  u.Id == userId));

                if(board == null)
                {
                    return NotFound();
                }

                var list = new List() 
                {
                    Title = createListDto.Title,
                    Board = board,
                    Tasks = []
                };

                await _context.AddAsync(list);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetList), new { listId = list.Id }, CreateListDto(list));
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        public async Task<List?> GetListById(int listId)
        {
            return await _context.Lists
                .SingleOrDefaultAsync(l => l.Id == listId);
        }

        private static ListDto CreateListDto(List list) => new()
        {
            Id = list.Id,
            Title = list.Title,
            Tasks = list.Tasks.Select(t => new TaskDto()
                {
                    Id = t.Id,
                    Title = t.Title
                }).ToList()
        };
    }
}