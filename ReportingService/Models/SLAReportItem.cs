using System;

namespace ReportingService.Models
{
    public class SLAReportItem
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public int DaysOverdue { get; set; }
        public DateTime DueDate { get; set; }
    }
}
