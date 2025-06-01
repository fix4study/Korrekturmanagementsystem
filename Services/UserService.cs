using Korrekturmanagementsystem.Constants;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleService _roleService;
    public UserService(IUserRepository userRepository, IRoleService roleService)
    {
        _userRepository = userRepository;
        _roleService = roleService;
    }

    public async Task<Result> CreateUser(CreateUserDto user)
    {
        var roleResult = await GetSystemRoleIdForUser(user);

        if (!roleResult.IsSuccess)
        {
            return Result.Failure($"Registrierung fehlgeschlagen: {roleResult.ErrorMessage}");
        }

        var result = await CheckUserConflictsAsync(user);
        if (!result.IsSuccess)
        {
            return result;
        }

        var userEntity = new User
        {
            Id = Guid.NewGuid(),
            Username = user.Username,
            Email = user.Email,
            Password = user.Password,
            StakeholderRoleId = user.StakeholderRoleId,
            SystemRoleId = roleResult.SystemRoleId!.Value,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await _userRepository.InsertAsync(userEntity);
            return Result.Success("Registrierung erfolgreich!");
        }
        catch (Exception ex)
        {
            return Result.Failure("Beim Speichern ist ein Fehler aufgetreten.");
        }
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user is null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            HashedPassword = user.Password,
            StakeholderRoleName = user.StakeholderRole.Name,
            SystemRoleName = user.SystemRole.Name
        };
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            StakeholderRoleName = user.StakeholderRole.Name
        };
    }

    private async Task<RoleResolutionResult> GetSystemRoleIdForUser(CreateUserDto user)
    {
        Models.Enums.SystemRole role = Models.Enums.SystemRole.User;

        if (user.StakeholderRoleId == StakeholderRoles.IUMitarbeiter)
        {
            if (!user.Email.EndsWith("@iu.org"))
            {
                return RoleResolutionResult.Failure("Für IU-Mitarbeiter muss eine IU-E-Mail-Adresse angegeben werden.");
            }

            role = Models.Enums.SystemRole.Intern;
        }

        if (user.StakeholderRoleId == StakeholderRoles.Student)
        {
            if (!user.Email.EndsWith("@iu-study.org"))
            {
                return RoleResolutionResult.Failure("Für Studierende muss eine @iu-study.org E-Mail-Adresse angegeben werden.");
            }

            role = Models.Enums.SystemRole.User;
        }

        var systemRoleId = await _roleService.GetSystemRoleIdByNameAsync(role.ToString());

        if (systemRoleId is null)
        {
            return RoleResolutionResult.Failure("Systemrolle konnte nicht zugeordnet werden.");
        }

        return RoleResolutionResult.Success(systemRoleId.Value);
    }

    private async Task<Result> CheckUserConflictsAsync(CreateUserDto user)
    {
        var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(user.Username);
        if (existingUserByUsername is not null)
        {
            return Result.Failure("Der Benutzername ist bereits vergeben.");
        }

        var existingEmail = await _userRepository.GetUserByEmailAsync(user.Email);
        if (existingEmail is not null)
        {
            return Result.Failure("Die E-Mail-Adresse ist bereits vergeben.");
        }

        return Result.Success();
    }
}
