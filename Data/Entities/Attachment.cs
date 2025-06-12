namespace Korrekturmanagementsystem.Data.Entities;

public class Attachment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReportId { get; set; }
    public string FileName { get; set; } = default!;
    public string FileUrl { get; set; } = default!;
    public DateTime UploadedAt { get; set; }

    public Report Report { get; set; } = default!;
}
