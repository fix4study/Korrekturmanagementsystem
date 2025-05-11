namespace Korrekturmanagementsystem.Dtos.Report;

public class UpdateReportDto : BaseReportDto
{
    public Guid Id { get; set; }
    public List<IFormFile>? Attachments { get; set; }
    public List<int>? TagIds { get; set; }
}
