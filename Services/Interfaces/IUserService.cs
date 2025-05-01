using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
}
