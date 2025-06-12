using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Korrekturmanagementsystem.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<User?> GetUserByEmailAsync(string email) =>
         await context.Users.Include(i => i.StakeholderRole).FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetUserByUsernameAsync(string username) =>
         await context.Users
            .Include(i => i.StakeholderRole)
            .Include(i => i.SystemRole)
            .FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User?> GetUserByIdAsync(Guid id) =>
         await context.Users
            .Include(i => i.StakeholderRole)
            .Include(i => i.SystemRole)
            .FirstOrDefaultAsync(u => u.Id == id);
}
