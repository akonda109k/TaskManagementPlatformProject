using Microsoft.AspNetCore.Mvc;
using TaskService.Models;
using TaskService.Services;
using System.Security.Claims;

namespace TaskService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult ListTasks([FromQuery] string? status, [FromQuery] Guid? assigneeId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            return Ok(_taskService.ListTasks(status, assigneeId, from, to));
        }

        [HttpGet("{id}")]
        public IActionResult GetTask(Guid id)
        {
            var task = _taskService.GetTask(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskItem task)
        {
            var created = _taskService.CreateTask(task);
            return CreatedAtAction(nameof(GetTask), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(Guid id, [FromBody] TaskItem task)
        {
            var changedBy = User.FindFirstValue(ClaimTypes.Name) ?? "unknown";
            var updated = _taskService.UpdateTask(id, task, changedBy);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
    }
}
