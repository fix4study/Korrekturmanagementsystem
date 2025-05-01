namespace Korrekturmanagementsystem.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}

