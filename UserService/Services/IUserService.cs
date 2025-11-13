using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        IEnumerable<User> ListUsers();
        User? GetUser(int id);
        User CreateUser(User user);
        User? UpdateUser(int id, User user);
        bool DeleteUser(int id);
        User? Authenticate(string username, string password);
    }
}
