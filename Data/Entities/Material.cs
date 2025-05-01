namespace Korrekturmanagementsystem.Data.Entities;

public class Material
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public int? MaterialTypeId { get; set; }
    public Guid CourseId { get; set; }

    public MaterialType MaterialType { get; set; } = default!;
    public Course? Course { get; set; }
    public ICollection<Report> Reports { get; set; } = default!;
}
