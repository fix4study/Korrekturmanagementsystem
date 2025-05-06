using Korrekturmanagementsystem.Data.Entities;

namespace Korrekturmanagementsystem.Repositories.Interfaces;

public interface IReportRepository : IBaseRepository<Report>
{
    Task<Report?> GetByIdAsync(Guid id);
}
