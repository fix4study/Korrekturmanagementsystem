using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IReportTagService
{
    Task InsertReportTagAsync(IEnumerable<ReportTagDto> reportTags);
    Task<IEnumerable<ReportTagDto>> GetReportTagsByReportIdAsync(Guid reportId);
    Task UpdateReportTagsAsync(Guid reportId, List<TagDto> reportTags);
}
