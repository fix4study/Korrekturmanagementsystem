using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;
    private readonly IUserRepository _userRepository;
    public UserService(IRepository<User> repository, IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);

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
        var users = await _repository.GetAsync(includes: e => e.StakeholderRole);

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

    public async Task CreateUser(CreateUserDto user)
    {

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
            await _repository.InsertAsync(userEntity);
        }
        catch (Exception ex)
        {
            var t = ex;
        }
    }
}
