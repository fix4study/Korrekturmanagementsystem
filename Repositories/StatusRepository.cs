using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;

namespace Korrekturmanagementsystem.Repositories;

public class StatusRepository : BaseRepository<Status>, IStatusRepository
{
    public StatusRepository(ApplicationDbContext context) : base(context) { }
}