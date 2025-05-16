using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IAttachmentProvider
{
    Task<bool> CreateAsync(CreateAttachmentDto attachment);
    Task<IEnumerable<AttachmentDto>> GetByReportIdAsync(Guid reportId);
}
