using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;

namespace Korrekturmanagementsystem.Repositories;

public class StakeholderRoleRepository : BaseRepository<StakeholderRole>, IStakeholderRoleRepository
{
    public StakeholderRoleRepository(ApplicationDbContext context) : base(context) { }
}
