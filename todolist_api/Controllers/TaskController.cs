using Microsoft.AspNetCore.Mvc;
using todolist_api.Models;
using todolist_api.Repositories;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController(IBaseRepository<Models.Task> tasks, IBaseRepository<Models.List> lists) : ControllerBase
    {
        public IBaseRepository<Models.Task> Tasks { get; } = tasks;
        public IBaseRepository<Models.List> Lists { get; } = lists;

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTask(int id)
        {
            try
            {
                var task = await Tasks.Get(id);

                return new JsonResult(new TaskDto() 
                {
                    Id = task.Id,
                    Title = task.Title
                });
            }
            catch(ArgumentException)
            {
                return NotFound("Task not found");
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewTask([FromQuery]int listId, [FromBody]TaskDto taskDto)
        {
            if(taskDto == null)
            {
                return BadRequest("Invalid task data");
            }

            try
            {
                var list = await Lists.Get(listId);

                var task = new Models.Task()
                {
                    Title = taskDto.Title,
                    List = list,
                };

                await Tasks.Create(task);

                return CreatedAtAction(nameof(GetTask), new { Id = task.Id }, new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title
                });
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