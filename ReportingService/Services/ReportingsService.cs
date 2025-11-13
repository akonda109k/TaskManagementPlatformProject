using ReportingService.Models;

namespace ReportingService.Services
{
    public class ReportingsService: IReportingService
    {
        // For demo, use in-memory data. In real scenario, fetch from DB or other services.
        private readonly List<dynamic> _tasks = new();

        public ReportingsService()
        {
            // Seed with sample tasks
            _tasks.Add(new { Id = Guid.NewGuid(), Title = "Sample Task", Status = Models.TaskStatus.Open, AssigneeId = Guid.NewGuid(), AssigneeName = "John", DueDate = DateTime.UtcNow.AddDays(-2) });
        }

        public IEnumerable<object> TasksByUser()
        {
            return _tasks.GroupBy(t => t.AssigneeId)
                .Select(g => new { UserId = g.Key, Count = g.Count() });
        }

        public IEnumerable<object> TasksByStatus()
        {
            return _tasks.GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() });
        }

        public IEnumerable<SLAReportItem> SLAReport()
        {
            var today = DateTime.UtcNow;
            return _tasks
                .Where(t => t.DueDate < today && t.Status != Models.TaskStatus.Completed)
                .Select(t => new SLAReportItem
                {
                    TaskId = t.Id,
                    Title = t.Title,
                    OwnerId = t.AssigneeId,
                    OwnerName = t.AssigneeName,
                    DaysOverdue = (today - t.DueDate).Days,
                    DueDate = t.DueDate
                }).ToList();
        }
    }
}
