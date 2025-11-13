using System;

namespace TaskService.Models
{
    public class ActivityLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string ChangedBy { get; set; } = string.Empty;
        public TaskStatus OldStatus { get; set; }
        public TaskStatus NewStatus { get; set; }
    }
}
