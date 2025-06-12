namespace Korrekturmanagementsystem.Dtos;

public class ReportTagDto
{
    public Guid ReportId { get; set; } = default!;
    public int TagId { get; set; } = default!;
    public string TagName { get; set; } = default!;
}
