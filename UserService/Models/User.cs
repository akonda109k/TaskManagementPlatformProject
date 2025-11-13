namespace UserService.Models
{
    public enum UserRole { Admin, Manager, Engineer }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // For stub only
        public UserRole Role { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
