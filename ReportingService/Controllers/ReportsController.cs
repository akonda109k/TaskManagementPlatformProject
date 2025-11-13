using Microsoft.AspNetCore.Mvc;
using ReportingService.Services;

namespace ReportingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportingService _reportingService;

        public ReportsController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet("tasks-by-user")]
        public IActionResult TasksByUser() => Ok(_reportingService.TasksByUser());

        [HttpGet("tasks-by-status")]
        public IActionResult TasksByStatus() => Ok(_reportingService.TasksByStatus());

        [HttpGet("sla-report")]
        public IActionResult SLAReport() => Ok(_reportingService.SLAReport());
    }
}
