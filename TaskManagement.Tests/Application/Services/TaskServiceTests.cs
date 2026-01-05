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

namespace TaskManagement.Tests.Application.Services
{
    public class TaskServiceTests
    {
        private readonly AppDbContext _context;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TaskTestDb")
                .Options;

            _context = new AppDbContext(options);

            // Seed a user
            var user = new User("testuser");
            _context.Users.Add(user);
            _context.SaveChanges();

            var taskRepo = new TaskRepository(_context);
            _taskService = new TaskService(taskRepo);
        }

        [Fact]
        public async Task CreateTask_Should_Add_Task()
        {
            var dto = new CreateTaskDto
            {
                Title = "Test Task",
                Description = "Test Description"
            };

            var userId = _context.Users.First().Id;
            var result = await _taskService.CreateAsync(dto, userId);

            result.Should().NotBeNull();
            result.Title.Should().Be("Test Task");
            var taskInDb = await _context.Tasks.FindAsync(result.Id);
            taskInDb.Should().NotBeNull();
        }

        [Fact]
        public async Task MarkCompleted_Should_Set_IsCompleted()
        {
            var dto = new CreateTaskDto { Title = "Complete Task" };
            var userId = _context.Users.First().Id;
            var task = await _taskService.CreateAsync(dto, userId);

            await _taskService.UpdateAsync(task.Id, new UpdateTaskDto
            {
                Title = task.Title,
                Description = task.Description,
                IsCompleted = true
            }, userId);

            var updatedTask = await _context.Tasks.FindAsync(task.Id);
            updatedTask!.IsCompleted.Should().BeTrue();
        }
    }
}
