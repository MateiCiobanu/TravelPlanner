using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Infrastructure.Persistence;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private DataContext _context;
        public LikeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CommentDislikedByUser(int userId, int commentId)
        {
            return await _context.Likes.AnyAsync(l => l.UserId == userId && (l.CommentId != null && l.CommentId == commentId) && l.Type == false);
        }

        public async Task<bool> CommentLikedByUser(int userId, int commentId)
        {
            return await _context.Likes.AnyAsync(l => l.UserId == userId && (l.CommentId != null && l.CommentId == commentId) && l.Type == true);
        }

        public async Task<bool> CommentLikeExists(int userId, int commentId)
        {
            return await _context.Likes.AnyAsync(l => l.UserId == userId && (l.CommentId != null && l.CommentId == commentId));
        }

        public async Task<bool> DeleteCommentLikes(ICollection<Like> likes)
        {
            _context.RemoveRange(likes);
            return await Save();
        }

        public async Task<int> GetCommentDislikes(int commentId)
        {
            return await _context.Likes.CountAsync(l => l.Type == false && l.CommentId == commentId);
        }

        public async Task<Like> GetCommentLikeByIds(int userId, int commentId)
        {
            return await _context.Likes.Where(l => l.UserId == userId && (l.CommentId != null && l.CommentId == commentId)).FirstOrDefaultAsync();
        }

        public async Task<int> GetCommentLikes(int commentId)
        {
            return await _context.Likes.CountAsync(l => l.Type == true && l.CommentId == commentId);
        }

        public async Task<ICollection<Like>> GetLikes()
        {
            return await _context.Likes.ToListAsync();
        }

        public async Task<ICollection<Like>> GetLikesByCommentId(int commentId)
        {
            return await _context.Likes.Where(l => l.CommentId != null && l.CommentId == commentId).ToListAsync();
        }

        public async Task<int> GetPostDislikes(int postId)
        {
            return await _context.Likes.CountAsync(l => l.Type == false && l.PostId == postId);
        }

        public async Task<Like> GetPostLikeByIds(int userId, int commentId)
        {
            return await _context.Likes.Where(l => l.UserId == userId && (l.PostId != null && l.PostId == commentId)).FirstOrDefaultAsync();
        }

        public async Task<int> GetPostLikes(int postId)
        {
            return await _context.Likes.CountAsync(l => l.Type == true && l.PostId == postId);
        }

        public async Task<bool> Like(Like like)
        {
            _context.Add(like);
            return await Save();
        }

        public async Task<bool> PostDislikedByUser(int userId, int postId)
        {
            return await _context.Likes.AnyAsync(l => l.UserId == userId && (l.PostId != null && l.PostId == postId) && l.Type == false);
        }

        public async Task<bool> PostLikedByUser(int userId, int postId)
        {
            return await _context.Likes.AnyAsync(l => l.UserId == userId && (l.PostId != null && l.PostId == postId) && l.Type == true);
        }

        public async Task<bool> PostLikeExists(int userId, int commentId)
        {
            return await _context.Likes.AnyAsync(l => l.UserId == userId && (l.PostId != null && l.PostId == commentId));
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> Unlike(Like like)
        {
            _context.Remove(like);
            return await Save();
        }

        public async Task<bool> UpdateLike(Like like)
        {
            _context.Update(like);
            return await Save();
        }
    }
}
