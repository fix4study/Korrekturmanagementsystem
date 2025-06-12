using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IReportTagService
{
    Task<Result> InsertReportTagAsync(IEnumerable<ReportTagDto> reportTags);
    Task<IEnumerable<ReportTagDto>> GetReportTagsByReportIdAsync(Guid reportId);
    Task<Result> UpdateReportTagsAsync(Guid reportId, List<TagDto> reportTags);
}
