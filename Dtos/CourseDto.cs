namespace Korrekturmanagementsystem.Dtos;

public class CourseDto
{
    public Guid Id { get; set; } = default;
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
}
