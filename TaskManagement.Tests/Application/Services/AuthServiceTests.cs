using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using Xunit;

namespace TaskManagement.Tests.Application.Services
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;
        private readonly Mock<IUserRepository> _userRepoMock = new();

        public AuthServiceTests()
        {
            _authService = new AuthService(_userRepoMock.Object);
        }

        [Fact]
        public async Task ValidateCredentialsAsync_Should_Return_True_When_Correct_Password()
        {
            // Arrange
            var user = new User("testuser");
            var hasher = new PasswordHasher<User>();
            var hash = hasher.HashPassword(user, "password123");

            // Use reflection to set the private property
            typeof(User)
                .GetProperty(nameof(User.PasswordHash))!
                .SetValue(user, hash);

            _userRepoMock.Setup(x => x.GetByUsernameAsync("testuser"))
                         .ReturnsAsync(user);

            // Act
            var result = await _authService.ValidateCredentialsAsync("testuser", "password123");

            // Assert
            result.Should().Be("testuser");
        }

        [Fact]
        public async Task ValidateCredentialsAsync_Should_Return_False_When_Wrong_Password()
        {
            // Arrange
            var user = new User("testuser");
            var hasher = new PasswordHasher<User>();
            var hash = hasher.HashPassword(user, "password123");

            // Use reflection to set the private property
            typeof(User)
                .GetProperty(nameof(User.PasswordHash))!
                .SetValue(user, hash);

            _userRepoMock.Setup(x => x.GetByUsernameAsync("testuser"))
                         .ReturnsAsync(user);

            // Act
            var result = await _authService.ValidateCredentialsAsync("testuser", "wrongpassword");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task ValidateCredentialsAsync_Should_Return_False_When_User_Not_Found()
        {
            // Arrange
            _userRepoMock.Setup(x => x.GetByUsernameAsync("unknown"))
                         .ReturnsAsync((User?)null);

            // Act
            var result = await _authService.ValidateCredentialsAsync("unknown", "any");

            // Assert
            result.Should().BeNull();
        }
    }
}
