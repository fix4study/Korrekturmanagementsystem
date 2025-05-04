using Microsoft.EntityFrameworkCore;

namespace Korrekturmanagementsystem.Data.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public Guid SystemRoleId { get; set; }
    public Guid StakeholderRoleId { get; set; }
    public DateTime CreatedAt { get; set; }


    public SystemRole SystemRole { get; set; } = default!;
    public StakeholderRole StakeholderRole { get; set; } = default!;
    public ICollection<Report> CreatedReports { get; set; } = default!;
    public ICollection<Comment> Comments { get; set; } = default!;
    public ICollection<ReportHistory> ChangedReports { get; set; } = default!;
}