using System;
using System.Collections.Generic;

namespace TaskService.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium"; // Low/Medium/High
        public TaskStatus Status { get; set; } = TaskStatus.Open;
        public Guid? AssigneeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public List<ActivityLogEntry> ActivityLog { get; set; } = new();
    }
}
