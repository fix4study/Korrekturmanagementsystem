using Korrekturmanagementsystem.Models;

namespace Korrekturmanagementsystem.Services.Interfaces;
public interface ICommentService
{
    Task<CommentViewModel> GetCommentsByReportIdAsync(Guid reportId);
    Task AddCommentAsync(Guid reportId, string content);
}

