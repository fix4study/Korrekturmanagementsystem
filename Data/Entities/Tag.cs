namespace Korrekturmanagementsystem.Data.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<ReportTag> ReportTags { get; set; } = default!;
}
