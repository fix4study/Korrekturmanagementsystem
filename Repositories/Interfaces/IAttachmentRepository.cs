using Korrekturmanagementsystem.Data.Entities;

namespace Korrekturmanagementsystem.Repositories.Interfaces;

public interface IAttachmentRepository : IBaseRepository<Attachment>
{
    Task<IEnumerable<Attachment>> GetAttachmentsByReportId(Guid reportId);
}
