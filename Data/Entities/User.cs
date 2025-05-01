using System.ComponentModel.DataAnnotations;

namespace Korrekturmanagementsystem.Data.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public Guid RoleId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Role Role { get; set; } = default!;
    public ICollection<Report> CreatedReports { get; set; } = default!;
    public ICollection<Comment> Comments { get; set; } = default!;
    public ICollection<ReportHistory> ChangedReports { get; set; } = default!;
}

