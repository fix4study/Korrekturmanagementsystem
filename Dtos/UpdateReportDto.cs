namespace Korrekturmanagementsystem.Dtos;

public class UpdateReportDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int ReportTypeId { get; set; }
    public int PriorityId { get; set; }
    public int MaterialTypeId { get; set; }
    public int StatusId { get; set; }
    public Guid? CourseId { get; set; }
    public List<IFormFile>? Attachments { get; set; }
    public List<int>? TagIds { get; set; }
}
