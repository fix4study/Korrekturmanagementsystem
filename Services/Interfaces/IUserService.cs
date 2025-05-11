using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<Result> CreateUser(CreateUserDto user);
    Task<UserDto?> GetUserByUsernameAsync(string username);
}
