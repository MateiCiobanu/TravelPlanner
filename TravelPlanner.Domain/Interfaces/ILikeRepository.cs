using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanner.Domain.Entities;

namespace TravelPlanner.Domain.Interfaces
{
    public interface ILikeRepository
    {
        Task<ICollection<Like>> GetLikes();
        Task<ICollection<Like>> GetLikesByCommentId(int commentId);
        Task<int> GetPostLikes(int postId);
        Task<int> GetPostDislikes(int postId);
        Task<int> GetCommentLikes(int commentId);
        Task<int> GetCommentDislikes(int commentId);
        Task<bool> CommentLikeExists(int userId, int commentId);
        Task<Like> GetCommentLikeByIds(int userId, int commentId);
        Task<bool> PostLikeExists(int userId, int commentId);
        Task<Like> GetPostLikeByIds(int userId, int commentId);
        Task<bool> PostLikedByUser(int userId, int postId);
        Task<bool> PostDislikedByUser(int userId, int postId);
        Task<bool> CommentLikedByUser(int userId, int commetId);
        Task<bool> CommentDislikedByUser(int userId, int commentId);
        Task<bool> Like(Like like);
        Task<bool> UpdateLike(Like like);
        Task<bool> Unlike(Like like);
        Task<bool> DeleteCommentLikes(ICollection<Like> likes);
        Task<bool> Save();
    }
}
