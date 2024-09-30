namespace todolist_api.Services
{
    public interface IAuthorizeService
    {
        public Task<bool> UserHasAccessToBoardAsync(int boardId, int userId);
        public Task<bool> UserHasAccessToListAsync(int listId, int userId);
        public Task<bool> UserHasAccessToTaskAsync(int taskId, int userId);
    }
}