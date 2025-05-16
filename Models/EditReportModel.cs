using Korrekturmanagementsystem.Dtos.Report;
using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Models;

public class EditReportModel
{
    public UpdateReportDto Report { get; set; } = new();
    public ReportFormOptionsDto Options { get; set; } = new();
    public List<AttachmentDto> Attachments { get; set; } = new();
    public List<TagDto> SelectedTags { get; set; } = new();
    public string CreatedByUsername { get; set; } = string.Empty;
}

