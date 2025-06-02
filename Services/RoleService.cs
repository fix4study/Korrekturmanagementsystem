using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class RoleService : IRoleService
{
    private readonly IStakeholderRoleRepository _stakeholderRoleRepository;
    private readonly ISystemRoleRepository _systemRoleRepository;

    public RoleService(IStakeholderRoleRepository stakeholderRoleRepository, ISystemRoleRepository systemRoleRepository)
    {
        _stakeholderRoleRepository = stakeholderRoleRepository;
        _systemRoleRepository = systemRoleRepository;
    }

    public async Task<IEnumerable<RoleDto>> GetStakeholderRolesAsync()
    {
        var roles = await _stakeholderRoleRepository.GetAllAsync();

        return roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        });
    }

    public async Task<Guid?> GetSystemRoleIdByNameAsync(string name)
        => await _systemRoleRepository.GetSystemRoleIdByNameAsync(name);
}
