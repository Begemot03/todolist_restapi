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
    public class TaskController : ControllerBase
    {

        private readonly ApplicationContext _context;
        private readonly IUserService _userService;

        public TaskController(ApplicationContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpGet("{taskId}")]
        [AuthorizeTask]
        public async Task<ActionResult> GetTask(int taskId)
        {
            try
            {
                var task = await GetTaskById(taskId);

                if(task == null)
                {
                    return NotFound();
                }

                return Ok(CreateTaskDto(task));
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
                var task = await GetTaskById(taskId);

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
        public async Task<ActionResult> UpdateTask(int taskId, [Required][FromBody] UpdateTaskDto updateTaskDto)
        {
            try
            {
                var userId = _userService.GetUserId();
                var list = await GetListById(updateTaskDto.ListId, userId);

                var task = await GetTaskById(taskId);

                if(task == null || list == null)
                {
                    return NotFound();
                }

                task.Title = updateTaskDto.Title;
                task.List = list;

                await _context.SaveChangesAsync();

                return Ok(CreateTaskDto(task));
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }



        [HttpPost]
        public async Task<ActionResult> CreateNewTask([Required][FromBody]CreateTaskDto createTaskDto)
        {
            try
            {
                var userId = _userService.GetUserId();
                var list = await GetListById(createTaskDto.ListId, userId);

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

                return CreatedAtAction(nameof(GetTask), new { task.Id }, CreateTaskDto(task));
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        private async Task<Models.Task?> GetTaskById(int taskId)
        {
            return await _context.Tasks.Where(t => t.Id == taskId).SingleOrDefaultAsync();
        }

        private async Task<List?> GetListById(int listId, int userId)
        {
            return await _context.Lists.SingleOrDefaultAsync(l => l.Id == listId && l.Board.Users.Any(u => u.Id == userId));
        }

        private TaskDto CreateTaskDto(Models.Task task) => new()
        {
            Id = task.Id,
            Title = task.Title
        };
    }
}