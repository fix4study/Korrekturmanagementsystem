using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Models;
using Korrekturmanagementsystem.Shared;
using Microsoft.AspNetCore.Components.Forms;

namespace Korrekturmanagementsystem.Services.Interfaces;

public interface IReportService
{
    Task<EditReportModel> BuildEditReportViewModelAsync(Guid reportId);
    Task<Result> UpdateReportAsync(EditReportModel model, List<IBrowserFile> files);
    Task<Result> AddReportAsync(AddReportDto report, List<TagDto> selectedTags, List<IBrowserFile> files);
    Task<ReportFormOptionsDto> GetFormOptionsAsync();
}
