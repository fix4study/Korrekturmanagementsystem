using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class AttachmentProvider : IAttachmentProvider
{
    private readonly IAttachmentRepository _attachmentRepository;
    public AttachmentProvider(IAttachmentRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
    }

    public async Task<IEnumerable<AttachmentDto>> GetByReportIdAsync(Guid reportId)
    {
        var attachments = await _attachmentRepository.GetAttachmentsByReportId(reportId);

        if(attachments == null || !attachments.Any())
        {
            return Enumerable.Empty<AttachmentDto>();
        }

        return attachments.Select(a => new AttachmentDto
        {
            Id = a.Id,
            FileName = a.FileName,
            FileUrl = a.FileUrl,
            UploadedAt = a.UploadedAt,
            ReportId = a.ReportId
        });
    }

    public async Task<bool> CreateAsync(CreateAttachmentDto attachment)
    {
        try
        {
            if (attachment == null)
            {
                return false;
            }

            var attachmentEntity = new Attachment
            {
                Id = Guid.NewGuid(),
                FileName = attachment.FileName,
                FileUrl = attachment.FileUrl,
                UploadedAt = DateTime.UtcNow,
                ReportId = attachment.ReportId
            };

            await _attachmentRepository.InsertAsync(attachmentEntity);

            return true;

        }
        catch (Exception ex)
        {
            return false;
        }

    }
}
