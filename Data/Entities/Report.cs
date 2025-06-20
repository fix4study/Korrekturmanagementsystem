namespace Korrekturmanagementsystem.Data.Entities;

public class Report
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public int ReportTypeId { get; set; }
    public int StatusId { get; set; }
    public int PriorityId { get; set; }
    public int MaterialTypeId { get; set; }
    public Guid? CourseId { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ReportType ReportType { get; set; } = default!;
    public Status Status { get; set; } = default!;
    public MaterialType MaterialType { get; set; } = default!;
    public Course? Course { get; set; }
    public Priority Priority { get; set; } = default!;
    public User CreatedBy { get; set; } = default!;
    public ICollection<ReportHistory> History { get; set; } = default!;
    public ICollection<Comment> Comments { get; set; } = default!;
    public ICollection<Attachment> Attachments { get; set; } = default!;
    public ICollection<ReportTag> ReportTags { get; set; } = default!;
}
