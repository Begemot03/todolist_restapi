using Microsoft.AspNetCore.Mvc;
using todolist_api.Models;
using todolist_api.Repositories;

namespace todolist_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController(IBaseRepository<List> lists, IBaseRepository<Board> boards) : ControllerBase
    {
        public IBaseRepository<List> Lists { get; set; } = lists;
        public IBaseRepository<Board> Boards { get; set; } = boards;

        [HttpGet("{id}")]
        public async Task<ActionResult> GetList(int id)
        {
            try
            {
                var list = await Lists.Get(id);

                return new JsonResult(new ListDto()
                {
                    Id = list.Id,
                    Title = list.Title
                });
            }
            catch(ArgumentException e)
            {
                return NotFound("List not found");
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewList([FromQuery]int boardId, [FromBody]CreateListDto createListDto)
        {
            if(createListDto == null)
            {
                return BadRequest("Invalid list data");
            }

            try
            {
                var board = await Boards.Get(boardId);

                var list = new List() 
                {
                    Title = createListDto.Title,
                    Board = board
                };

                await Lists.Create(list);

                return CreatedAtAction(nameof(GetList), new { list.Id }, list);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }

        }
    }
}