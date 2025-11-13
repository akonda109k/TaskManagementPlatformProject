using MongoDB.Driver;
using UserService.Data;
using UserService.Models;
using UserService.Services;

public class UsersService : IUserService
{
    private readonly IMongoCollection<User> _users;

    public UsersService(MongoDbContext context)
    {
        _users = context.Database.GetCollection<User>("Users");
    }

    public IEnumerable<User> ListUsers() => _users.Find(_ => true).ToList();

    public User? GetUser(int id) => _users.Find(u => u.Id == id).FirstOrDefault();

    public User CreateUser(User user)
    {
        _users.InsertOne(user);
        return user;
    }

    public User? UpdateUser(int id, User user)
    {
        var result = _users.ReplaceOne(u => u.Id == id, user);
        return result.IsAcknowledged && result.ModifiedCount > 0 ? user : null;
    }

    public bool DeleteUser(int id)
    {
        var result = _users.DeleteOne(u => u.Id == id);
        return result.DeletedCount > 0;
    }

    public User? Authenticate(string username, string password)
    {
        return _users.Find(u => u.Username == username && u.Password == password).FirstOrDefault();
    }
}
