using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class ReportTagProvider : IReportTagProvider
{
    private readonly IReportTagRepository _reportTagRepository;
    public ReportTagProvider(IReportTagRepository reportTagRepository)
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

    public async Task<IEnumerable<ReportTagDto>> GetReportTagsByReportIdAsync(Guid reportId)
    {
        var reportTags = await _reportTagRepository.GetReportTagsByReportIdAsync(reportId);

        return reportTags.Select(reportTag => new ReportTagDto
        {
            ReportId = reportTag.ReportId,
            TagId = reportTag.TagId,
            TagName = reportTag.Tag.Name
        });
    }
}
