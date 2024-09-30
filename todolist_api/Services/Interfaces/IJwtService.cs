namespace todolist_api
{
    public interface IJwtService
    {
        public string GenerateToken(int userId);
    }
}