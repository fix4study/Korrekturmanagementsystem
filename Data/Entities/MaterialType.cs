namespace Korrekturmanagementsystem.Data.Entities;

public class MaterialType
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<Report> Reports { get; set; } = default!;
}
