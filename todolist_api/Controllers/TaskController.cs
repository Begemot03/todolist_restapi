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
    public class TaskController : ControllerBase
    {

        private readonly ApplicationContext _context;

        public TaskController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("{taskId}")]
        [AuthorizeTask]
        public async Task<ActionResult> GetTask(int taskId)
        {
            try
            {
                var task = await _context.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync();

                if(task == null)
                {
                    return NotFound();
                }
                
                var taskDto = new TaskDto()
                {
                    Id = task.Id,
                    Title = task.Title
                };

                return Ok(task);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpDelete("{taskId}")]
        [AuthorizeTask]
        public async Task<ActionResult> DeleteTask(int taskId)
        {
            try
            {
                var task = await _context.Tasks.Where(t => t.Id == taskId).FirstOrDefaultAsync();

                if(task == null)
                {
                    return NotFound();
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpPut("{taskId}")]
        [AuthorizeTask]
        public async Task<ActionResult> UpdateTask(int taskId, [FromBody] UpdateTaskDto updateTaskDto)
        {
            try
            {
                if(updateTaskDto == null)
                {
                    return BadRequest("Invalid task data");
                }

                var userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var list = await _context.Lists
                    .Where(l => l.Id == updateTaskDto.ListId && l.Board.Users.Any(u => u.Id == userId))
                    .FirstOrDefaultAsync();

                if(list == null)
                {
                    return NotFound();
                }

                var task = await _context.Tasks
                    .Where(t => t.Id == taskId)
                    .FirstOrDefaultAsync();

                if(task == null)
                {
                    return NotFound();
                }

                task.Title = updateTaskDto.Title;
                task.List = list;

                await _context.SaveChangesAsync();

                var taskDto = new TaskDto()
                {
                    Id = task.Id,
                    Title = task.Title
                };

                return Ok(taskDto);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }



        [HttpPost]
        public async Task<ActionResult> CreateNewTask([FromBody]CreateTaskDto createTaskDto)
        {
            if(createTaskDto == null)
            {
                return BadRequest("Invalid task data");
            }

            try
            {
                var userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                var list = await _context.Lists.SingleOrDefaultAsync(l => l.Id == createTaskDto.ListId && l.Board.Users.Any(u => u.Id == userId));

                if(list == null)
                {
                    return NotFound();
                }

                var task = new Models.Task()
                {
                    Title = createTaskDto.Title,
                    List = list,
                };

                await _context.Tasks.AddAsync(task);
                await _context.SaveChangesAsync();

                var taskDto = new TaskDto()
                {
                    Id = task.Id,
                    Title = task.Title
                };

                return CreatedAtAction(nameof(GetTask), new { Id = task.Id }, taskDto);
            }
            catch(ArgumentException e)
            {
                return NotFound($"{e.Message}");
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}