using Microsoft.EntityFrameworkCore;
using todolist_api.Database;
using todolist_api.Models;

namespace todolist_api.Repositories
{
    public class UserLoginRepository
    {
        private readonly ApplicationContext _context;

        public UserLoginRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> AddNewUser(UserAuthDto userDto)
        {

            var user = new User()
            {
                Username = userDto.Username,
                PasswordHash = userDto.PasswordHash
            };

            await _context.Set<User>().AddAsync(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> IsUserExists(UserAuthDto userDto)
        {
            try
            {
                var isExist = await _context.Users.AnyAsync(user => user.Username == userDto.Username);

                return isExist;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsPasswordCorrect(UserAuthDto userDto)
        {
            try
            {
                var user = await _context.Users.SingleAsync(user => user.Username == userDto.Username);

                if(user != null)
                {
                    return user.PasswordHash == userDto.PasswordHash;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}