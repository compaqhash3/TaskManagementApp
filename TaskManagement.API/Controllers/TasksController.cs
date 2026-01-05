using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(int userId)
        {
            var tasks = await _taskService.GetTasksAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("{userId}/{id}")]
        public async Task<IActionResult> GetById(int userId, int id)
        {
            var task = await _taskService.GetByIdAsync(id, userId);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> Create(int userId, [FromBody] CreateTaskDto dto)
        {
            var task = await _taskService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { userId, id = task.Id }, task);
        }

        [HttpPut("{userId}/{id}")]
        public async Task<IActionResult> Update(int userId, int id, [FromBody] UpdateTaskDto dto)
        {
            await _taskService.UpdateAsync(id, dto, userId);
            return NoContent();
        }

        [HttpDelete("{userId}/{id}")]
        public async Task<IActionResult> Delete(int userId, int id)
        {
            await _taskService.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}
