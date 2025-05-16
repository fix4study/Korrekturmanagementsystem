using Korrekturmanagementsystem.Components.Pages;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Providers.Interfaces;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Providers;

public class CommentProvider : ICommentProvider
{
    private readonly IBaseRepository<Comment> _commentRepository;
    private readonly IUserProvider _userProvider;
    public CommentProvider(IBaseRepository<Comment> commentRepository, IUserProvider userProvider)
    {
        _commentRepository = commentRepository;
        _userProvider = userProvider;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsByReportIdAsync(Guid reportId)
    {
        var commentsEntity = await _commentRepository.GetAllAsync(c => c.ReportId == reportId);

        var comments = new List<CommentDto>();
        foreach (var comment in commentsEntity)
        {
            var user = await _userProvider.GetUserByIdAsync(comment.AuthorId);

            comments.Add(new CommentDto
            {
                Id = comment.Id,
                Author = user.Username,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            });
        }

        return comments;
    }
}
