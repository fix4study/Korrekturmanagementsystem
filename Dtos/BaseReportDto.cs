namespace Korrekturmanagementsystem.Dtos;

public class BaseReportDto
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int ReportTypeId { get; set; }
    public int PriorityId { get; set; }
    public int MaterialTypeId { get; set; }
    public Guid? CourseId { get; set; }
    public int StatusId { get; set; }
}
