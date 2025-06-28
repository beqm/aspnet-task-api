namespace Domain.Models;

public class Task
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool Complete { get; set; } = false;
    public Guid UserId { get; private set; }
    public User? User { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Task(Guid userId, string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        }

        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Description = description;
        Complete = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Task Create(Guid userId, string title, string description)
    {
        return new Task(userId, title, description);
    }

    public void Completed()
    {
        if (Complete)
        {
            throw new InvalidOperationException("Task is already completed.");
        }

        Complete = true;
        UpdatedAt = DateTime.UtcNow;
        CompletedAt = DateTime.UtcNow;
    }

    public void Uncompleted()
    {
        if (Complete)
        {
            throw new InvalidOperationException("Task is already completed.");
        }

        Complete = true;
        UpdatedAt = DateTime.UtcNow;
        CompletedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string? newTitle = null, string? newDescription = null)
    {
        if (Complete)
            throw new InvalidOperationException("Cannot update a completed task.");

        bool updated = false;

        if (!string.IsNullOrWhiteSpace(newTitle) && newTitle != Title)
        {
            Title = newTitle;
            updated = true;
        }

        if (newDescription != null && newDescription != Description)
        {
            Description = newDescription;
            updated = true;
        }

        if (updated)
            UpdatedAt = DateTime.UtcNow;
    }
}