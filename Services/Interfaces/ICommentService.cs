using Korrekturmanagementsystem.Models;

namespace Korrekturmanagementsystem.Services.Interfaces;
public interface ICommentService
{
    Task<CommentModel> GetCommentsByReportIdAsync(Guid reportId);
    Task AddCommentAsync(Guid reportId, string content);
}
