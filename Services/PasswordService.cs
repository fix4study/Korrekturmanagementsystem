using Korrekturmanagementsystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Korrekturmanagementsystem.Services;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _hasher = new();

    public string HashPassword(string password)
        => _hasher.HashPassword(null, password);

    public bool VerifyPassword(string hashedPassword, string providedPassword)
        => _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
}
