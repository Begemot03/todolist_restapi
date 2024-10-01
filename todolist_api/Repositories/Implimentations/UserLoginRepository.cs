using Microsoft.EntityFrameworkCore;
using todolist_api.Database;
using todolist_api.Models;
using todolist_api.Services;

namespace todolist_api.Repositories
{
    public class UserLoginRepository
    {
        private readonly ApplicationContext _context;
        private readonly IPasswordHasherService _passwordHasherService;

        public UserLoginRepository(ApplicationContext context, IPasswordHasherService passwordHasherService)
        {
            _context = context;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<User> AddNewUser(UserAuthDto userDto)
        {

            var user = new User()
            {
                Username = userDto.Username,
                Password = _passwordHasherService.HashPassword(userDto.Password)
            };

            await _context.Set<User>().AddAsync(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetUser(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> IsUserExists(UserAuthDto userDto)
        {
            return await GetUser(userDto.Username) != null;
        }

        public async Task<bool> IsDataCorrect(UserAuthDto userDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Username == userDto.Username);

            if(user == null)
            {
                return false;
            }

            return _passwordHasherService.VerifyPassword(user.Password, userDto.Password);
        }
    }
}