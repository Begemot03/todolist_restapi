using Microsoft.AspNetCore.Identity;

namespace todolist_api.Services
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<object> _passwordHasher;

        public PasswordHasherService()
        {
            _passwordHasher = new PasswordHasher<object>();
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            var isVerify = _passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
            
            return isVerify == PasswordVerificationResult.Success;
        }
    }
}