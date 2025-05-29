using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanner.Domain.Entities;

namespace TravelPlanner.Domain.Interfaces
{
    public interface ICommentRepository
    {
        Task<bool> CreateComment(Comment comment);
        Task<bool> DeleteComment(Comment comment);
        Task<bool> CommentExists(int id);
        Task<Comment> GetCommentById(int id);
        Task<bool> Save();
        Task<IEnumerable<Comment>> GetCommentsByPostId(int postId);

    }
}
