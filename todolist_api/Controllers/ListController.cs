using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todolist_api.Database;
using todolist_api.Filters;
using todolist_api.Models;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ListController(ApplicationContext context)
        {
            _context = context;
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

                var listDto = new ListDto()
                {
                    Id = list.Id,
                    Title = list.Title,
                    Tasks = list.Tasks.Select(t => new TaskDto()
                    {
                        Id = t.Id,
                        Title = t.Title
                    }).ToList()
                };

                return new JsonResult(listDto);
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
                var list = await _context.Lists.Where(l => l.Id == listId).FirstOrDefaultAsync();

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
        public async Task<ActionResult> UpdateList(int listId, [FromBody] UpdateListDto updateListDto)
        {
            try
            {
                var list = await _context.Lists
                    .Where(l => l.Id == listId)
                    .FirstOrDefaultAsync();
                
                if(list == null)
                {
                    return NotFound();
                }

                list.Title = updateListDto.Title;
                await _context.SaveChangesAsync();

                var listDto = new ListDto()
                {
                    Id = list.Id,
                    Title = list.Title
                };

                return Ok(listDto);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewList([FromBody]CreateListDto createListDto)
        {
            if(createListDto == null)
            {
                return BadRequest("Invalid list data");
            }

            try
            {
                var userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

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

                var listDto = new ListDto()
                {
                    Id = list.Id,
                    Title = list.Title,
                    Tasks = list.Tasks.Select(t => new TaskDto()
                    {
                        Id = t.Id,
                        Title = t.Title
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetList), new { list.Id }, listDto);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}