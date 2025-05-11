namespace Korrekturmanagementsystem.Dtos.Report;

public class ReportOverviewDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public string StatusName { get; set; } = default!;
    public string PriorityName { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
