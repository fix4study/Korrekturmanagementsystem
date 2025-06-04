using Korrekturmanagementsystem.Constants;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Shared;
using Korrekturmanagementsystem.UnitOfWork;

namespace Korrekturmanagementsystem.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoleService _roleService;

    public UserService(IUnitOfWork unitOfWork, IRoleService roleService)
    {
        _unitOfWork = unitOfWork;
        _roleService = roleService;
    }

    public async Task<Result> CreateUser(CreateUserDto user)
    {
        var systemRoleResult = await GetSystemRoleIdForUser(user);

        if (!systemRoleResult.IsSuccess)
        {
            return Result.Failure($"Registrierung fehlgeschlagen: {systemRoleResult.Message}");
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
            SystemRoleId = systemRoleResult.Data,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await _unitOfWork.Users.InsertAsync(userEntity);
            return Result.Success("Registrierung erfolgreich!");
        }
        catch (Exception ex)
        {
            return Result.Failure("Beim Speichern ist ein Fehler aufgetreten.");
        }
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _unitOfWork.Users.GetUserByUsernameAsync(username);

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
        var user = await _unitOfWork.Users.GetUserByIdAsync(id);

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

    private async Task<Result<Guid>> GetSystemRoleIdForUser(CreateUserDto user)
    {
        Models.Enums.SystemRole role = Models.Enums.SystemRole.User;

        if (user.StakeholderRoleId == StakeholderRoles.IUMitarbeiter)
        {
            if (!user.Email.EndsWith("@iu.org"))
            {
                return Result<Guid>.Failure("Für IU-Mitarbeiter muss eine IU-E-Mail-Adresse angegeben werden.");
            }

            role = Models.Enums.SystemRole.Intern;
        }

        if (user.StakeholderRoleId == StakeholderRoles.Student)
        {
            if (!user.Email.EndsWith("@iu-study.org"))
            {
                return Result<Guid>.Failure("Für Studierende muss eine @iu-study.org E-Mail-Adresse angegeben werden.");
            }

            role = Models.Enums.SystemRole.User;
        }

        var systemRoleId = await _roleService.GetSystemRoleIdByNameAsync(role.ToString());

        if (systemRoleId is null)
        {
            return Result<Guid>.Failure("Systemrolle konnte nicht zugeordnet werden.");
        }

        return Result<Guid>.Success(systemRoleId.Value);
    }

    private async Task<Result> CheckUserConflictsAsync(CreateUserDto user)
    {
        var existingUserByUsername = await _unitOfWork.Users.GetUserByUsernameAsync(user.Username);
        if (existingUserByUsername is not null)
        {
            return Result.Failure("Der Benutzername ist bereits vergeben.");
        }

        var existingEmail = await _unitOfWork.Users.GetUserByEmailAsync(user.Email);
        if (existingEmail is not null)
        {
            return Result.Failure("Die E-Mail-Adresse ist bereits vergeben.");
        }

        return Result.Success();
    }
}
