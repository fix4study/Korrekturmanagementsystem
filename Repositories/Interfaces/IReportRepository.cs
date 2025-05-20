using Korrekturmanagementsystem.Data.Entities;

namespace Korrekturmanagementsystem.Repositories.Interfaces;

public interface IReportRepository : IBaseRepository<Report>
{
    Task<Report?> GetByIdAsync(Guid id);
    Task UpdateAsync();
    Task<IEnumerable<Report>> GetAllByUserIdAsync(Guid userId);
    Task<Guid?> GetCreatorIdByReportIdAsync(Guid id);
}
