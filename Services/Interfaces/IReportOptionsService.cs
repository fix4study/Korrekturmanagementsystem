using Korrekturmanagementsystem.Dtos.Report;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IReportOptionsService
{
    Task<ReportFormOptionsDto> GetFormOptionsAsync();
}
