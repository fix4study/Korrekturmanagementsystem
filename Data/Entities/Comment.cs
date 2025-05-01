namespace Korrekturmanagementsystem.Data.Entities;

public class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ReportId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public Report Report { get; set; } = default!;
    public User Author { get; set; } = default!;
}
