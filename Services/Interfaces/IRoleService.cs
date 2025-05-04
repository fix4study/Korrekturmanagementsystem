using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IRoleService
{
    Task AddStakeholderRoleAsync(string name);
    Task<IEnumerable<RoleDto>> GetStakeholderRolesAsync();
}
