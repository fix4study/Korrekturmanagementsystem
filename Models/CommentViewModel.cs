using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Models;

public class CommentViewModel
{
    public List<CommentDto> Comments { get; set; } = new();
}
