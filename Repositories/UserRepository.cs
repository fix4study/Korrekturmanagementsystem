using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Korrekturmanagementsystem.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email) =>
         await _context.Users.Include(i => i.StakeholderRole).FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetUserByUsernameAsync(string username) =>
         await _context.Users
            .Include(i => i.StakeholderRole)
            .Include(i => i.SystemRole)
            .FirstOrDefaultAsync(u => u.Username == username);

}
