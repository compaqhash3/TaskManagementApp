using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.DTOs
{
    public record TaskDto
    (
     int Id,
     string Title,
     string? Description,
     bool IsCompleted,
     bool IsDeleted
    );
}
