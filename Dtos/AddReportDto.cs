namespace Korrekturmanagementsystem.Dtos;

public class AddReportDto : BaseReportDto
{
    public List<IFormFile>? Attachments { get; set; }
    public List<int>? TagIds { get; set; }

}
