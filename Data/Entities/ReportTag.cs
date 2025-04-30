namespace Korrekturmanagementsystem.Data.Entities;

public class ReportTag
{
    public Guid ReportId { get; set; }
    public int TagId { get; set; }

    public Report Report { get; set; } = default!;
    public Tag Tag { get; set; } = default!;
}
