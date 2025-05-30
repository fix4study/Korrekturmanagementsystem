using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class RoleService : IRoleService
{
    private readonly IRoleProvider _roleProvider;

    public RoleService(IRoleProvider roleProvider)
    {
        _roleProvider = roleProvider;
    }

    public Task<IEnumerable<RoleDto>> GetStakeholderRolesAsync()
        => _roleProvider.GetStakeholderRolesAsync();

    public Task<Guid?> GetSystemRoleIdByNameAsync(string name)
        => _roleProvider.GetSystemRoleIdByNameAsync(name);
}
