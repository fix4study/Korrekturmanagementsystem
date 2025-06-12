namespace Korrekturmanagementsystem.Data.Entities;

public class ReportHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReportId { get; set; }
    public int StatusId { get; set; }
    public Guid ChangedById { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? Note { get; set; }

    public Report Report { get; set; } = default!;
    public Status Status { get; set; } = default!;
    public User ChangedBy { get; set; } = default!;
}
