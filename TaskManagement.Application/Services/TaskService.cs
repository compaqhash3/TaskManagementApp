using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksAsync(int userId)
        {
            var tasks = await _taskRepository.GetAllAsync(userId);

            return tasks.Select(task => new TaskDto(
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.IsDeleted
            ));
        }

        public async Task<TaskDto?> GetByIdAsync(int id, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(id, userId);
            if (task is null) return null;

            return new TaskDto(
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.IsDeleted
            );
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto, int userId)
        {
            var task = new TaskItem(
                dto.Title,
                userId,
                dto.Description
            );

            await _taskRepository.AddAsync(task);

            return new TaskDto(
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.IsDeleted
            );
        }

        public async Task UpdateAsync(int id, UpdateTaskDto dto, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(id, userId)
                       ?? throw new KeyNotFoundException("Task not found");

            task.Update(dto.Title, dto.Description);

            if (dto.IsCompleted)
                task.MarkCompleted();

            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var task = await _taskRepository.GetByIdAsync(id, userId)
                       ?? throw new KeyNotFoundException("Task not found");

            task.Delete();

            await _taskRepository.DeleteAsync(task);
        }
    }
}
