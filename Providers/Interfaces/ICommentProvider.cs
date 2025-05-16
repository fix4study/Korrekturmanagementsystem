using Korrekturmanagementsystem.Dtos;

namespace Korrekturmanagementsystem.Providers.Interfaces;

public interface ICommentProvider
{
    Task<IEnumerable<CommentDto>> GetCommentsByReportIdAsync(Guid reportId);
    Task AddCommentAsync(Guid reportId, string content);
}
