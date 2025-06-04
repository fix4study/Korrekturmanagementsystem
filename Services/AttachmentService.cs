using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.UnitOfWork;

namespace Korrekturmanagementsystem.Services;

public class AttachmentService : IAttachmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AttachmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AttachmentDto>> GetByReportIdAsync(Guid reportId)
    {
        var attachments = await _unitOfWork.Attachments.GetAttachmentsByReportId(reportId);

        if (attachments?.Any() != true)
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
            if (attachment is null)
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

            await _unitOfWork.Attachments.InsertAsync(attachmentEntity);

            return true;

        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
