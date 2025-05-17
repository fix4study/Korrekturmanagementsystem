namespace Korrekturmanagementsystem.Dtos;

public class ReportHistoryDto
{
    public string StatusName { get; set; } = default!;
    public string ChangedByName { get; set; } = default!;
    public DateTime ChangedAt { get; set; }
    public string? Note { get; set; } = default!;
}
