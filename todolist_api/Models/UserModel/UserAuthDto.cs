namespace todolist_api.Models
{
    public class UserAuthDto
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}