using UserService.Models;

namespace UserService.Services
{
    public class UsersService : IUserService
    {
        private readonly List<User> _users = new();

        public UsersService()
        {
            // Seed with one admin user
            _users.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Password = "admin", // For stub only
                Role = UserRole.Admin,
                Email = "admin@org.com"
            });
        }

        public IEnumerable<User> ListUsers() => _users;

        public User? GetUser(Guid id) => _users.FirstOrDefault(u => u.Id == id);

        public User CreateUser(User user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
            return user;
        }

        public User? UpdateUser(Guid id, User user)
        {
            var existing = GetUser(id);
            if (existing == null) return null;
            existing.Username = user.Username;
            existing.Password = user.Password;
            existing.Role = user.Role;
            existing.Email = user.Email;
            return existing;
        }

        public bool DeleteUser(Guid id)
        {
            var user = GetUser(id);
            if (user == null) return false;
            _users.Remove(user);
            return true;
        }

        public User? Authenticate(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
