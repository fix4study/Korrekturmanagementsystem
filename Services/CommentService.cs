using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Dtos;
using Korrekturmanagementsystem.Models;
using Korrekturmanagementsystem.Repositories.Interfaces;
using Korrekturmanagementsystem.Services.Interfaces;

namespace Korrekturmanagementsystem.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUserService;
    public CommentService(ICommentRepository commentRepository, 
        IUserService userProvider, 
        ICurrentUserService currentUserService)
    {
        _commentRepository = commentRepository;
        _userService = userProvider;
        _currentUserService = currentUserService;
    }

    public async Task<CommentModel> GetCommentsByReportIdAsync(Guid reportId)
    {
        var commentsEntity = await _commentRepository.GetAllAsync(c => c.ReportId == reportId);

        var comments = new List<CommentDto>();
        foreach (var comment in commentsEntity)
        {
            var user = await _userService.GetUserByIdAsync(comment.AuthorId);

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

        return new CommentModel
        {
            Comments = comments.ToList()
        };
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
