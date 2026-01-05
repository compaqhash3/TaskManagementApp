using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; private set; } = null!;
        public string? Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeleted { get; set; } = false;
        public int UserId { get; private set; }

        // EF Core
        private TaskItem() { }

        public TaskItem(string title, int userId, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty");

            Title = title;
            UserId = userId;
            Description = description;            
            IsCompleted = false;
            IsDeleted = false;
        }

        public void Update(string title, string? description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty");

            Title = title;
            Description = description;
        }

        public void MarkCompleted()
        {
            IsCompleted = true;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
