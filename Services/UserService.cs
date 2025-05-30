using Korrekturmanagementsystem.Constants;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Models.Enums;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Services;

public class UserService : IUserService
{
    private readonly IUserProvider _userProvider;
    private readonly IRoleService _roleService;
    public UserService(IUserProvider userProvider, IRoleService roleService)
    {
        _userProvider = userProvider;
        _roleService = roleService;
    }

    public async Task<Result> CreateUser(CreateUserDto user)
    {
        var roleResult = await GetSystemRoleIdForUser(user);

        if (!roleResult.IsSuccess)
        {
            return Result.Failure($"Registrierung fehlgeschlagen: {roleResult.ErrorMessage}");
        }

        var result = await _userProvider.CreateUser(user, roleResult.SystemRoleId!.Value);

        return result;
    }

    private async Task<RoleResolutionResult> GetSystemRoleIdForUser(CreateUserDto user)
    {
        SystemRole role = SystemRole.User;

        if (user.StakeholderRoleId == StakeholderRoles.IUMitarbeiter)
        {
            if (!user.Email.EndsWith("@iu.org"))
            {
                return RoleResolutionResult.Failure("Für IU-Mitarbeiter muss eine IU-E-Mail-Adresse angegeben werden.");
            }

            role = SystemRole.Intern;
        }

        if (user.StakeholderRoleId == StakeholderRoles.Student)
        {
            if (!user.Email.EndsWith("@iu-study.org"))
            {
                return RoleResolutionResult.Failure("Für Studierende muss eine @iu-study.org E-Mail-Adresse angegeben werden.");
            }

            role = SystemRole.User;
        }

        var systemRoleId = await _roleService.GetSystemRoleIdByNameAsync(role.ToString());

        if (systemRoleId is null)
        {
            return RoleResolutionResult.Failure("Systemrolle konnte nicht zugeordnet werden.");
        }

        return RoleResolutionResult.Success(systemRoleId.Value);
    }
}
