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

    public async Task<CommentViewModel> GetCommentsByReportIdAsync(Guid reportId)
    {
        var result = await _commentProvider.GetCommentsByReportIdAsync(reportId);

        return new CommentViewModel
        {
            Comments = result.ToList()
        };
    }
}
