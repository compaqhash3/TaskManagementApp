using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetTasksAsync(int userId);
        Task<TaskDto?> GetByIdAsync(int id, int userId);
        Task<TaskDto> CreateAsync(CreateTaskDto dto, int userId);
        Task UpdateAsync(int id, UpdateTaskDto dto, int userId);
        Task DeleteAsync(int id, int userId);
    }
}
