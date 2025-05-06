using System.ComponentModel.DataAnnotations;

namespace Korrekturmanagementsystem.Dtos;
public class AddReportDto
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int ReportTypeId { get; set; }
    public int PriorityId { get; set; }
    public int MaterialTypeId { get; set; }
    public Guid? CourseId { get; set; }
    public List<IFormFile>? Attachments { get; set; }
    public List<int>? TagIds { get; set; }

}
