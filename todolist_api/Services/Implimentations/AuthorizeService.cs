using Microsoft.EntityFrameworkCore;
using todolist_api.Database;

namespace todolist_api.Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly ApplicationContext _context;

        public AuthorizeService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> UserHasAccessToBoardAsync(int boardId, int userId)
        {
            var board = await _context.Boards
                .AsNoTracking()
                .Include(b => b.Users)
                .FirstOrDefaultAsync(b => b.Id == boardId);

            if(board == null)
            {
                return false;
            }

            return board.Users.Any(u => u.Id == userId);
        }

        public async Task<bool> UserHasAccessToListAsync(int listId, int userId)
        {
            var list = await _context.Lists
                .AsNoTracking()
                .Include(l => l.Board)
                .ThenInclude(b => b.Users)
                .FirstOrDefaultAsync(l => l.Id == listId);

            if(list == null)
            {
                return false;
            }

            return list.Board.Users.Any(u => u.Id == userId);
        }
        
        public async Task<bool> UserHasAccessToTaskAsync(int taskId, int userId)
        {
            var task = await _context.Tasks
                .AsNoTracking()
                .Include(t => t.List)
                .ThenInclude(l => l.Board)
                .ThenInclude(b => b.Users)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if(task == null)
            {
                return false;
            }

            return task.List.Board.Users.Any(u => u.Id == userId);
        }
    }
}