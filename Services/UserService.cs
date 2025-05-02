using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;
    private readonly IRepository<Role> _roleRepository;
    private readonly IUserRepository _userRepository;
    public UserService(IRepository<User> repository, IRepository<Role> roleRepository, IUserRepository userRepository)
    {
        _repository = repository;
        _roleRepository = roleRepository;
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
            RoleName = user.Role.Name
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _repository.GetAsync(includes: e => e.Role);

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            RoleName = user.Role.Name
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
            RoleName = user.Role.Name
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
            RoleId = user.RoleId,
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

    public async Task<IEnumerable<RoleDto>> GetAllUserRoles()
    {
        var roles = await _roleRepository.GetAsync();

        return roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name
        }).ToList();
    }
}
