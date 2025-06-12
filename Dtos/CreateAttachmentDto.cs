namespace Korrekturmanagementsystem.Dtos;

public class CreateAttachmentDto
{
    public Guid ReportId { get; set; }

    /// <summary>
    /// Der ursprüngliche Dateiname des Uploads
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// Öffentliche oder gesicherte URL des gespeicherten Blobs
    /// </summary>
    public string FileUrl { get; set; } = default!;

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
