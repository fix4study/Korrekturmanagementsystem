using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Models;

public class CommentModel
{
    public List<CommentDto> Comments { get; set; } = new();
}
