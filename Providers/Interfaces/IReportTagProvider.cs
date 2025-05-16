using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IReportTagProvider
{
    Task InsertReportTagAsync(IEnumerable<ReportTagDto> reportTags);
    Task<IEnumerable<ReportTagDto>> GetReportTagsByReportIdAsync(Guid reportId);
}
