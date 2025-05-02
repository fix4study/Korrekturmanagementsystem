namespace Korrekturmanagementsystem.Dtos;

public class CreateUserDto
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public Guid RoleId { get; set; }
    public DateTime CreatedAt { get; set; }
}
