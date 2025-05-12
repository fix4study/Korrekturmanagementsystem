using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class ReportTagService : IReportTagService
{
    private readonly IBaseRepository<ReportTag> _reportTagRepository;
    public ReportTagService(IBaseRepository<ReportTag> reportTagRepository)
    {
        _reportTagRepository = reportTagRepository;
    }

    public async Task InsertReportTagAsync(IEnumerable<ReportTagDto> reportTags)
    {
        foreach (var reportTag in reportTags)
        {
            await _reportTagRepository.InsertAsync(new ReportTag
            {
                ReportId = reportTag.ReportId,
                TagId = reportTag.TagId
            });
        }
    }
}
