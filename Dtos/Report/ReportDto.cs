using System.ComponentModel.DataAnnotations;

namespace Korrekturmanagementsystem.Dtos.Report;

public class ReportDto
{
    public Guid Id { get; set; }
    public int StatusId { get; set; }

    [Required(ErrorMessage = "Bitte wählen Sie einen Titel.")]
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    [Required(ErrorMessage = "Bitte wählen Sie einen Meldungstyp.")]
    public int? ReportTypeId { get; set; } = null;

    public int? PriorityId { get; set; }

    [Required(ErrorMessage = "Bitte wählen Sie ein Material.")]
    public int? MaterialTypeId { get; set; }
    public Guid? CourseId { get; set; }
    public List<int>? TagIds { get; set; }
}
