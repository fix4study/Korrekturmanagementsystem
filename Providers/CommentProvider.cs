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
    private readonly ICurrentUserService _currentUserService;
    public CommentProvider(IBaseRepository<Comment> commentRepository, IUserProvider userProvider, ICurrentUserService currentUserService)
    {
        _commentRepository = commentRepository;
        _userProvider = userProvider;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsByReportIdAsync(Guid reportId)
    {
        var commentsEntity = await _commentRepository.GetAllAsync(c => c.ReportId == reportId);

        var comments = new List<CommentDto>();
        foreach (var comment in commentsEntity)
        {
            var user = await _userProvider.GetUserByIdAsync(comment.AuthorId);

            if (user is null)
            {
                continue;
            }

            comments.Add(new CommentDto
            {
                Id = comment.Id,
                Author = user.Username,
                StakeholderRoleName = user.StakeholderRoleName,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            });
        }

        return comments;
    }

    public async Task AddCommentAsync(Guid reportId, string content)
    {
        var userId = _currentUserService.GetCurrentUserId();

        if (userId is null)
        {
            return;
        }

        var newComment = new Comment
        {
            Id = new Guid(),
            AuthorId = userId.Value,
            Content = content,
            ReportId = reportId,
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepository.InsertAsync(newComment);
    }
}
