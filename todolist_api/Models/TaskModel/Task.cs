namespace todolist_api.Models
{
    public class Task : BaseModel
    {
        public string Title { get; set; }
        public List List { get; set; }
    }
}