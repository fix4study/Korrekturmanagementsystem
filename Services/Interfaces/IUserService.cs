using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IUserService
{
    Task<string> CreateUser(CreateUserDto user);
}
