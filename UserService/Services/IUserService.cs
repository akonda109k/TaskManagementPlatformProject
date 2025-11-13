using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        IEnumerable<User> ListUsers();
        User? GetUser(Guid id);
        User CreateUser(User user);
        User? UpdateUser(Guid id, User user);
        bool DeleteUser(Guid id);
        User? Authenticate(string username, string password);
    }
}
