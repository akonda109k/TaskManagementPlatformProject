using Microsoft.AspNetCore.Mvc;
using Moq;
using ReportingService.Controllers;
using ReportingService.Models;
using ReportingService.Services;

/// <summary>
/// Contains unit tests for <see cref="ReportsController"/>.
/// </summary>
public class ReportsControllerTests
{
    /// <summary>
    /// Verifies that <see cref="ReportsController.TasksByUser"/> returns an OkObjectResult with the expected data.
    /// </summary>
    [Fact]
    public void TasksByUser_ReturnsOk_WithData()
    {
        var mockService = new Mock<IReportingService>();
        var expected = new List<object> { new { UserId = Guid.NewGuid(), Count = 5 } };
        mockService.Setup(s => s.TasksByUser()).Returns(expected);

        var controller = new ReportsController(mockService.Object);

        var result = controller.TasksByUser();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, okResult.Value);
    }

    /// <summary>
    /// Verifies that <see cref="ReportsController.TasksByStatus"/> returns an OkObjectResult with the expected data.
    /// </summary>
    [Fact]
    public void TasksByStatus_ReturnsOk_WithData()
    {
        var mockService = new Mock<IReportingService>();
        var expected = new List<object> { new { Status = "Open", Count = 10 } };
        mockService.Setup(s => s.TasksByStatus()).Returns(expected);

        var controller = new ReportsController(mockService.Object);

        var result = controller.TasksByStatus();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, okResult.Value);
    }

    /// <summary>
    /// Verifies that <see cref="ReportsController.SLAReport"/> returns an OkObjectResult with the expected SLA report data.
    /// </summary>
    [Fact]
    public void SLAReport_ReturnsOk_WithData()
    {
        var mockService = new Mock<IReportingService>();
        var expected = new List<SLAReportItem>
        {
            new SLAReportItem
            {
                TaskId = Guid.NewGuid(),
                Title = "Test Task",
                OwnerId = Guid.NewGuid(),
                OwnerName = "Alice",
                DaysOverdue = 2,
                DueDate = DateTime.UtcNow.AddDays(-2)
            }
        };
        mockService.Setup(s => s.SLAReport()).Returns(expected);

        var controller = new ReportsController(mockService.Object);

        var result = controller.SLAReport();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, okResult.Value);
    }
}
