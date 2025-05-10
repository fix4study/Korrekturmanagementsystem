using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IAttachmentService
{
    Task<bool> CreateAsync(CreateAttachmentDto attachment);
    Task<IEnumerable<AttachmentDto>> GetByReportIdAsync(Guid reportId);
}
