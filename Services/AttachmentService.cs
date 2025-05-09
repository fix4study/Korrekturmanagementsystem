using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class AttachmentService : IAttachmentService
{
    private readonly IBaseRepository<Attachment> _attachmentRepository;
    public AttachmentService(IBaseRepository<Attachment> attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
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
