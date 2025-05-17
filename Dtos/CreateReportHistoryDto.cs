namespace Korrekturmanagementsystem.Dtos;

public class CreateReportHistoryDto
{
    public Guid ReportId { get; set; }
    public int StatusId { get; set; }
    public string? Note { get; set; }
}
