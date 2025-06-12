using Korrekturmanagementsystem.Data;
using Korrekturmanagementsystem.Data.Entities;
using Korrekturmanagementsystem.Repositories.Interfaces;

namespace Korrekturmanagementsystem.Repositories;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context) { }
}
