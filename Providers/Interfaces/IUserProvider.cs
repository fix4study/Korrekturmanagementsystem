using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IUserProvider
{
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<Result> CreateUser(CreateUserDto user, Guid systemRoleId);
    Task<UserDto?> GetUserByUsernameAsync(string username);
}
