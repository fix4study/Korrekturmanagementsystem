namespace Korrekturmanagementsystem.Dtos;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string StakeholderRoleName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

