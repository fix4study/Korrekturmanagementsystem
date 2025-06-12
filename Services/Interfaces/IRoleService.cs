using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetStakeholderRolesAsync();
    Task<Guid?> GetSystemRoleIdByNameAsync(string name);
}
