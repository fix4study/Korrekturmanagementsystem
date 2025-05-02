using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task CreateUser(CreateUserDto user);
    Task<IEnumerable<RoleDto>> GetAllUserRoles();
    Task<UserDto?> GetUserByUsernameAsync(string username);
}
