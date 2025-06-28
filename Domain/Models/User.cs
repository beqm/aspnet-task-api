namespace Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = "user";
    public ICollection<Task> Tasks { get; set; } = new List<Task>();

    private User() { }

    public User(string username, string password, string role)
    {
        Id = Guid.NewGuid();
        Username = username;
        Password = password;
        Role = role;
    }

    public static User Create(string username, string password, string role)
    {
        return new User(username, password, role);
    }
}