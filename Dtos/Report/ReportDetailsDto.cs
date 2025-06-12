namespace Korrekturmanagementsystem.Dtos.Report;

public class ReportDetailsDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public ReportTypeDto ReportType { get; set; } = default!;
    public StatusDto Status { get; set; } = default!;
    public PriorityDto Priority { get; set; } = default!;
    public MaterialTypeDto MaterialType { get; set; } = default!;
    public CourseDto? Course { get; set; }
    public string CreatedByUsername { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
