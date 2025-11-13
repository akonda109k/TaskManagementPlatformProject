using TaskService.Models;

namespace TaskService.Services
{
    public interface ITaskService
    {
        IEnumerable<TaskItem> ListTasks(string? status, Guid? assigneeId, DateTime? from, DateTime? to);
        TaskItem? GetTask(Guid id);
        TaskItem CreateTask(TaskItem task);
        TaskItem? UpdateTask(Guid id, TaskItem task, string changedBy);
    }
}
