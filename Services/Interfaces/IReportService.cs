using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Models;
using Korrekturmanagementsystem.Shared;

using Microsoft.AspNetCore.Components.Forms;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IReportService
{
    Task<ReportModel?> BuildEditReportViewModelAsync(Guid reportId);
    Task<Result<Guid>> UpdateReportAsync(ReportModel model, List<IBrowserFile> files);
    Task<Result<Guid>> AddReportAsync(ReportModel model, List<TagDto> selectedTags, List<IBrowserFile> files);
    Task<IEnumerable<ReportOverviewDto>> GetAllReportsAsync();
    Task<IEnumerable<ReportOverviewDto>> GetAllReportByUserIdAsync();
    Task<Guid?> GetCreatorUserIdByReportIdAsync(Guid id);
}
