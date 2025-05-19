using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Models;

public class ReportModel
{
    public string StatusNote { get; set; } = string.Empty;
    public ReportDto Report { get; set; } = new();
    public ReportFormOptionsDto Options { get; set; } = new();
    public List<AttachmentDto> Attachments { get; set; } = new();
    public List<TagDto> SelectedTags { get; set; } = new();
    public string CreatedByUsername { get; set; } = string.Empty;
    public IEnumerable<ReportHistoryDto> ReportHistory { get; set; } = default!;
}

