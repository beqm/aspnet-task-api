namespace Application.Dtos;


public class UserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
}

public class UserWithTasksDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public List<TaskDto> Tasks { get; set; } = new();
}
