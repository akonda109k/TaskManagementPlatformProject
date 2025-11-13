using TaskService.Models;

namespace TaskService.Services
{
    public class TasksService: ITaskService
    {
        private readonly List<TaskItem> _tasks = new();

        public IEnumerable<TaskItem> ListTasks(string? status, Guid? assigneeId, DateTime? from, DateTime? to)
        {
            var query = _tasks.AsQueryable();
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<TaskService.Models.TaskStatus>(status, out var stat))
                query = query.Where(t => t.Status == stat);
            if (assigneeId.HasValue)
                query = query.Where(t => t.AssigneeId == assigneeId.Value);
            if (from.HasValue)
                query = query.Where(t => t.CreatedAt >= from.Value);
            if (to.HasValue)
                query = query.Where(t => t.CreatedAt <= to.Value);
            return query.ToList();
        }

        public TaskItem? GetTask(Guid id) => _tasks.FirstOrDefault(t => t.Id == id);

        public TaskItem CreateTask(TaskItem task)
        {
            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            _tasks.Add(task);
            return task;
        }

        public TaskItem? UpdateTask(Guid id, TaskItem updated, string changedBy)
        {
            var task = GetTask(id);
            if (task == null) return null;

            if (task.Status != updated.Status)
            {
                task.ActivityLog.Add(new ActivityLogEntry
                {
                    Timestamp = DateTime.UtcNow,
                    ChangedBy = changedBy,
                    OldStatus = task.Status,
                    NewStatus = updated.Status
                });
            }

            task.Title = updated.Title;
            task.Description = updated.Description;
            task.Priority = updated.Priority;
            task.Status = updated.Status;
            task.AssigneeId = updated.AssigneeId;
            task.UpdatedAt = DateTime.UtcNow;
            task.DueDate = updated.DueDate;
            return task;
        }
    }
}
