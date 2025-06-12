using Korrekturmanagementsystem.Data.Entities;

namespace Korrekturmanagementsystem.Repositories.Interfaces;

public interface ISystemRoleRepository : IBaseRepository<SystemRole>
{
    Task<Guid?> GetSystemRoleIdByNameAsync(string name);
}
