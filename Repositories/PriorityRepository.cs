using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;

namespace Korrekturmanagementsystem.Repositories;

public class PriorityRepository : BaseRepository<Priority>, IPriorityRepository
{
    public PriorityRepository(ApplicationDbContext context) : base(context) { }
}
