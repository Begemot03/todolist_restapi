namespace todolist_api.Models
{
    public class List : BaseModel
    {
        public string Title { get; set; }
        public Board Board { get; set; }
        public List<Task> Tasks { get; set; } = [];
    }
}