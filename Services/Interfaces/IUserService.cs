using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IUserService
{
    Task<Result> CreateUser(CreateUserDto user);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<UserDto?> GetUserByIdAsync(Guid id);
}
