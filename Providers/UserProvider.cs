using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Services;

public class UserProvider : IUserProvider
{
    private readonly IUserRepository _userRepository;
    public UserProvider(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            StakeholderRoleName = user.StakeholderRole.Name
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync(includes: e => e.StakeholderRole);

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            StakeholderRoleName = user.StakeholderRole.Name
        }).ToList();
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

    public async Task<Result> CreateUser(CreateUserDto user)
    {
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
            SystemRoleId = Guid.Parse("a937147a-86b6-4af7-bbff-c8d04741e411"), //Standard User Role, Todo: Select User Role
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await _userRepository.InsertAsync(userEntity);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Beim Speichern ist ein Fehler aufgetreten.");
        }
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
