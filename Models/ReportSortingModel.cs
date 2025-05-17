using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Models.Enums;

namespace Korrekturmanagementsystem.Models;

public class ReportSortingModel
{
    public SortField CurrentSortField = SortField.CreatedAt;
    public bool SortDescending = true;
    public void SetSort(SortField field)
    {
        if (CurrentSortField == field)
        {
            SortDescending = !SortDescending;
        }
        else
        {
            CurrentSortField = field;
            SortDescending = true;
        }
    }

    public IEnumerable<ReportOverviewDto> SortedReports(IEnumerable<ReportOverviewDto> reports)
    {
        return CurrentSortField switch
        {
            SortField.CreatedAt => SortDescending
                ? reports.OrderByDescending(r => r.CreatedAt)
                : reports.OrderBy(r => r.CreatedAt),
            SortField.UpdatedAt => SortDescending
                ? reports.OrderByDescending(r => r.UpdatedAt)
                : reports.OrderBy(r => r.UpdatedAt),
            _ => reports
        };
    }
}
