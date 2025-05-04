using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class RoleService : IRoleService
{
    private readonly IRepository<StakeholderRole> _roleRepository;
    public RoleService(IRepository<StakeholderRole> repository)
    {
        _roleRepository = repository;
    }

    public async Task AddStakeholderRoleAsync(string name)
    {
        var role = new StakeholderRole
        {
            Name = name
        };

        await _roleRepository.InsertAsync(role);
    }

    public async Task<IEnumerable<RoleDto>> GetStakeholderRolesAsync()
    {
        var roles = await _roleRepository.GetAsync();

        return roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        });
    }
}
