namespace Korrekturmanagementsystem.Data.Entities;

public class Course
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;

    public ICollection<Report> Reports { get; set; } = default!;
}
