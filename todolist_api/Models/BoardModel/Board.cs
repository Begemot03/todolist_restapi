namespace todolist_api.Models
{
    public class Board : BaseModel
    {
        public string Title { get; set; }
        public List<User> Users { get; } = [];
        public List<List> Lists { get; set; } = [];
    }
}