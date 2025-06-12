namespace Korrekturmanagementsystem.Dtos;

public class AttachmentDto
{
    public Guid Id { get; set; }
    public Guid ReportId { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public DateTime UploadedAt { get; set; }
}
