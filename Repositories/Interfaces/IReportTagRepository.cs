using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Repositories.Interfaces;

public interface IReportTagRepository : IBaseRepository<ReportTag>
{
    Task<IEnumerable<ReportTag>> GetReportTagsByReportIdAsync(Guid reportId);
    Task<bool> DeleteByReportIdAsync(Guid reportId);
    new Task<Result<Guid>> InsertAsync(ReportTag entity);
}
