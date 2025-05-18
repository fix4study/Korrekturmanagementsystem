using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IRoleProvider
{
    Task AddStakeholderRoleAsync(string name);
    Task<IEnumerable<RoleDto>> GetStakeholderRolesAsync();
    Task<Guid?> GetSystemRoleIdByNameAsync(string name);
}
