using Korrekturmanagementsystem.Models;
using Korrekturmanagementsystem.Providers.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class CommentService : ICommentService
{
    readonly ICommentProvider _commentProvider;
    public CommentService(ICommentProvider commentProvider)
    {
        _commentProvider = commentProvider;
    }

    public async Task<CommentModel> GetCommentsByReportIdAsync(Guid reportId)
    {
        var result = await _commentProvider.GetCommentsByReportIdAsync(reportId);

        return new CommentModel
        {
            Comments = result.ToList()
        };
    }

    public async Task AddCommentAsync(Guid reportId, string content) =>
        await _commentProvider.AddCommentAsync(reportId, content);
    
}
