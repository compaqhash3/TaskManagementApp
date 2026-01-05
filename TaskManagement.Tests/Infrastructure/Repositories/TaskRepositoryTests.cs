using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Tests.Infrastructure.Repositories
{
    public class TaskRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly TaskRepository _repository;
        private readonly User _user;

        public TaskRepositoryTests()
        {
            // Setup InMemory DB
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Seed a user
            _user = new User("testuser");
            _context.Users.Add(_user);
            _context.SaveChanges();

            _repository = new TaskRepository(_context);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Task()
        {
            var task = new TaskItem("Test Task", _user.Id);

            await _repository.AddAsync(task);

            var tasks = await _repository.GetAllAsync(_user.Id);
            tasks.Should().ContainSingle();
            tasks.First().Title.Should().Be("Test Task");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_Task()
        {
            var task = new TaskItem("Task 1", _user.Id);
            await _repository.AddAsync(task);

            var fetched = await _repository.GetByIdAsync(task.Id, _user.Id);
            fetched.Should().NotBeNull();
            fetched!.Title.Should().Be("Task 1");
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Task()
        {
            var task = new TaskItem("Old Title", _user.Id);
            await _repository.AddAsync(task);

            task.Update("New Title", null);
            await _repository.UpdateAsync(task);

            var updated = await _repository.GetByIdAsync(task.Id, _user.Id);
            updated!.Title.Should().Be("New Title");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Task()
        {
            // Arrange
            var task = new TaskItem("Delete Me", _user.Id);
            await _repository.AddAsync(task);

            // Act (IMPORTANT: mark as deleted first)
            task.Delete();                 // 👈 this is the missing line
            await _repository.DeleteAsync(task);

            // Assert 1: Task should NOT appear in active list
            var tasks = await _repository.GetAllAsync(_user.Id);
            tasks.Should().BeEmpty();

            // Assert 2: Task should still exist but marked deleted
            var deletedTask = await _context.Tasks
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.Id == task.Id);

            deletedTask.Should().NotBeNull();
            deletedTask!.IsDeleted.Should().BeTrue();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
