namespace Korrekturmanagementsystem.Data.Entities;

public class StakeholderRole
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<User> Users { get; set; } = default!;
}
