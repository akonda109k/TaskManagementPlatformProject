using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskService.Controllers;
using TaskService.Models;
using TaskService.Services;

public class TasksControllerTests
{
    private readonly Mock<ITaskService> _mockService;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _mockService = new Mock<ITaskService>();
        _controller = new TasksController(_mockService.Object);
    }

    [Fact]
    public void ListTasks_ReturnsOk_WithTasks()
    {
        var tasks = new List<TaskItem> { new TaskItem { Id = Guid.NewGuid() } };
        _mockService.Setup(s => s.ListTasks(null, null, null, null)).Returns(tasks);

        var result = _controller.ListTasks(null, null, null, null);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(tasks, okResult.Value);
    }

    [Fact]
    public void GetTask_ReturnsOk_WhenTaskExists()
    {
        var id = Guid.NewGuid();
        var task = new TaskItem { Id = id };
        _mockService.Setup(s => s.GetTask(id)).Returns(task);

        var result = _controller.GetTask(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(task, okResult.Value);
    }

    [Fact]
    public void GetTask_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.GetTask(id)).Returns((TaskItem?)null);

        var result = _controller.GetTask(id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void CreateTask_ReturnsCreatedAtAction_WithCreatedTask()
    {
        var task = new TaskItem { Id = Guid.NewGuid() };
        _mockService.Setup(s => s.CreateTask(task)).Returns(task);

        var result = _controller.CreateTask(task);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetTask), createdResult.ActionName);
        Assert.Equal(task.Id, ((Guid)createdResult.RouteValues["id"]));
        Assert.Equal(task, createdResult.Value);
    }

    [Fact]
    public void UpdateTask_ReturnsOk_WhenTaskUpdated()
    {
        var id = Guid.NewGuid();
        var task = new TaskItem { Id = id };
        var updatedTask = new TaskItem { Id = id, Title = "Updated" };
        _mockService.Setup(s => s.UpdateTask(id, task, "tester")).Returns(updatedTask);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "tester") }));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = _controller.UpdateTask(id, task);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(updatedTask, okResult.Value);
    }

    [Fact]
    public void UpdateTask_ReturnsNotFound_WhenTaskNotUpdated()
    {
        var id = Guid.NewGuid();
        var task = new TaskItem { Id = id };
        _mockService.Setup(s => s.UpdateTask(id, task, "tester")).Returns((TaskItem?)null);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "tester") }));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var result = _controller.UpdateTask(id, task);

        Assert.IsType<NotFoundResult>(result);
    }
}
