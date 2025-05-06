namespace Korrekturmanagementsystem.Dtos;

public class ReportFormOptionsDto
{
    public List<ReportTypeDto> ReportTypes { get; set; } = new();
    public List<PriorityDto> Priorities { get; set; } = new();
    public List<MaterialTypeDto> MaterialTypes { get; set; } = new();
    public List<CourseDto> Courses { get; set; } = new();
}
