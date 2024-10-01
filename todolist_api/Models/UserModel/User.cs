namespace todolist_api.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Board> Boards { get; } = [];
    }
}