using Microsoft.AspNetCore.Identity;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> ValidateCredentialsAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user is null) return null;

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                password
            );

            return result == PasswordVerificationResult.Success ? user : null;
        }
    }
}
