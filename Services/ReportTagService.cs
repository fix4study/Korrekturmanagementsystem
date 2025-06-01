using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class ReportTagService : IReportTagService
{
    private readonly IReportTagRepository _reportTagRepository;
    public ReportTagService(IReportTagRepository reportTagRepository)
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

    public async Task UpdateReportTagsAsync(Guid reportId, List<TagDto> reportTags)
    {
        try
        {
            var success = await _reportTagRepository.DeleteByReportIdAsync(reportId);

            if (!success)
            {
                return;
            }

            var reportTagToInsert = reportTags
                .Select(tag => new ReportTagDto
                {
                    ReportId = reportId,
                    TagId = tag.Id
                })
                .ToList();

            if (!reportTagToInsert.Any())
            {
                return;
            }

            await InsertReportTagAsync(reportTagToInsert);
        }
        catch (Exception ex)
        {
            return;
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
