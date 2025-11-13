using ReportingService.Models;

namespace ReportingService.Services
{
    public interface IReportingService
    {
        IEnumerable<object> TasksByUser();
        IEnumerable<object> TasksByStatus();
        IEnumerable<SLAReportItem> SLAReport();
    }
}
