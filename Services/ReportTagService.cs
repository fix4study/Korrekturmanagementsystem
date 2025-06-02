using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Services.Interfaces;
using Korrekturmanagementsystem.Shared;

namespace Korrekturmanagementsystem.Services;

public class ReportTagService : IReportTagService
{
    private readonly IUnitOfWork _unitOfWork;
    public ReportTagService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> InsertReportTagAsync(IEnumerable<ReportTagDto> reportTags)
    {
        foreach (var reportTag in reportTags)
        {
            var result = await _unitOfWork.ReportTags.InsertAsync(new ReportTag
            {
                ReportId = reportTag.ReportId,
                TagId = reportTag.TagId
            });

            if (!result.IsSuccess) 
            {
                return Result.Failure("Beim Hinzufügen eines Tags ist ein Fehler aufgetreten");
            }
        }

        return Result.Success();
    }

    public async Task<Result> UpdateReportTagsAsync(Guid reportId, List<TagDto> reportTags)
    {
        try
        {
            var success = await _unitOfWork.ReportTags.DeleteByReportIdAsync(reportId);

            if (!success)
            {
                return Result.Failure("Bei der Aktualisierung der Tags ist ein Fehler aufgetreten");
            }

            var reportTagToInsert = reportTags
                .Select(tag => new ReportTagDto
                {
                    ReportId = reportId,
                    TagId = tag.Id
                })
                .ToList();

            if (reportTagToInsert.Any())
            {
                await InsertReportTagAsync(reportTagToInsert);
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Beim Speichern der Tags ist ein Fehler aufgetreten.");
        }
    }

    public async Task<IEnumerable<ReportTagDto>> GetReportTagsByReportIdAsync(Guid reportId)
    {
        var reportTags = await _unitOfWork.ReportTags.GetReportTagsByReportIdAsync(reportId);

        return reportTags.Select(reportTag => new ReportTagDto
        {
            ReportId = reportTag.ReportId,
            TagId = reportTag.TagId,
            TagName = reportTag.Tag.Name
        });
    }
}
