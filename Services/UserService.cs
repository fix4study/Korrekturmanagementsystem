using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;
    public UserService(IRepository<User> repository)
    {
        _repository = repository;
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
        var users =  await _repository.GetAsync(includes:e => e.Role);

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            RoleName = user.Role.Name
        }).ToList();
    }
}
