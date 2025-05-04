namespace Korrekturmanagementsystem.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string HashedPassword { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string StakeholderRoleName { get; set; } = default!;
    public string SystemRoleName { get; set; } = default!;
}

