using System.ComponentModel.DataAnnotations;

namespace Korrekturmanagementsystem.Dtos;
public class AddReportDto
{
    [Required]
    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    [Required]
    public int ReportTypeId { get; set; }
    public int PriorityId { get; set; }

    [Required]
    public int MaterialTypeId { get; set; }
    public Guid? CourseId { get; set; }
    public List<IFormFile>? Attachments { get; set; }
    public List<int>? TagIds { get; set; }

}
