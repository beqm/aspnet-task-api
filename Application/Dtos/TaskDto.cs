namespace Application.Dtos;

public class TaskDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string UserId { get; set; }
    public string? Description { get; set; }
    public required bool Complete { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}