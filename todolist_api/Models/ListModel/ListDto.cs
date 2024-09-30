namespace todolist_api.Models
{
    public class ListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<TaskDto> Tasks { get; set; }
    }
}