using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IReportTagService
{
    Task InsertReportTagAsync(IEnumerable<ReportTagDto> reportTags);
}
