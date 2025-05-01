namespace Korrekturmanagementsystem.Data.Entities;

public class Role
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;

    public ICollection<User> Users { get; set; } = default!;
}
