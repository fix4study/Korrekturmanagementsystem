using Korrekturmanagementsystem.Dtos.Report;

namespace Korrekturmanagementsystem.Models;

public class ReportFilterModel
{
    public Action? OnChanged { get; set; }

    private string _title = "";
    public string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                OnChanged?.Invoke();
            }
        }
    }

    private string _status = "";
    public string Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnChanged?.Invoke();
            }
        }
    }


    private string _priority = "";
    public string Priority
    {
        get => _priority;
        set
        {
            if (_priority != value)
            {
                _priority = value;
                OnChanged?.Invoke();
            }
        }
    }

    public List<ReportOverviewDto> Apply(IEnumerable<ReportOverviewDto> reports)
    {
        return reports
            .Where(r => string.IsNullOrWhiteSpace(Title) || r.Title.Contains(Title, StringComparison.OrdinalIgnoreCase))
            .Where(r => string.IsNullOrWhiteSpace(Status) || r.StatusName == Status)
            .Where(r => string.IsNullOrWhiteSpace(Priority) || r.PriorityName == Priority)
            .ToList();
    }
}