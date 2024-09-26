namespace todolist_api.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public List<Board> Boards { get; } = [];
    }
}