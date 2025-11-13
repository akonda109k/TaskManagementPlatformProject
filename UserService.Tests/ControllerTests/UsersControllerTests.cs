using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.Controllers;
using UserService.Models;
using UserService.Services;

public class UsersControllerTests
{
    private UsersController CreateControllerWithRole(string role, Mock<IUserService>? userServiceMock = null)
    {
        var service = userServiceMock ?? new Mock<IUserService>();
        var controller = new UsersController(service.Object);

        var claims = new List<Claim> { new Claim(ClaimTypes.Role, role) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };

        return controller;
    }

    [Fact]
    public void ListUsers_ReturnsOkResult_WithUsers()
    {
        var users = new List<User> { new User { Id = 1, Username = "admin" } };
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.ListUsers()).Returns(users);

        var controller = CreateControllerWithRole("Admin", mockService);

        var result = controller.ListUsers();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(users, okResult.Value);
    }

    [Fact]
    public void GetUser_ReturnsOkResult_WhenUserExists()
    {
        var user = new User { Id = 1, Username = "admin" };
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetUser(1)).Returns(user);

        var controller = CreateControllerWithRole("Admin", mockService);

        var result = controller.GetUser(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(user, okResult.Value);
    }

    [Fact]
    public void GetUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.GetUser(2)).Returns((User)null);

        var controller = CreateControllerWithRole("Admin", mockService);

        var result = controller.GetUser(2);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void CreateUser_ReturnsCreatedAtAction_ForAdmin()
    {
        var user = new User { Id = 2, Username = "user" };
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.CreateUser(user)).Returns(user);

        var controller = CreateControllerWithRole("Admin", mockService);

        var result = controller.CreateUser(user);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(user, createdResult.Value);
    }

    [Fact]
    public void UpdateUser_ReturnsOkResult_WhenUpdated()
    {
        var user = new User { Id = 2, Username = "user" };
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.UpdateUser(2, user)).Returns(user);

        var controller = CreateControllerWithRole("Admin", mockService);

        var result = controller.UpdateUser(2, user);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(user, okResult.Value);
    }

    [Fact]
    public void UpdateUser_ReturnsNotFound_WhenUserNotFound()
    {
        var user = new User { Id = 2, Username = "user" };
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.UpdateUser(2, user)).Returns((User)null);

        var controller = CreateControllerWithRole("Admin", mockService);

        var result = controller.UpdateUser(2, user);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteUser_ReturnsNoContent_ForAdmin_WhenDeleted()
    {
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.DeleteUser(2)).Returns(true);

        var controller = CreateControllerWithRole("Admin", mockService);

        var result = controller.DeleteUser(2);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteUser_ReturnsNotFound_ForAdmin_WhenNotFound()
    {
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.DeleteUser(2)).Returns(false);

        var controller = CreateControllerWithRole("Admin", mockService);

        var result = controller.DeleteUser(2);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void DeleteUser_ReturnsForbid_ForNonAdmin()
    {
        var mockService = new Mock<IUserService>();
        var controller = CreateControllerWithRole("User", mockService);

        var result = controller.DeleteUser(2);

        var forbidResult = Assert.IsType<ForbidResult>(result);
    }
}
