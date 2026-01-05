using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.DTOs
{
    public record UpdateTaskDto
    {
        public required string Title { get; init; }
        public string? Description { get; init; }
        public bool IsCompleted { get; init; }
    }
}
