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
    public class CommentRepository : ICommentRepository
    {
        private DataContext _context;
        public CommentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CommentExists(int id)
        {
            return await _context.Comments.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateComment(Comment comment)
        {
            _context.Add(comment);
            return await Save();
        }

        public async Task<bool> DeleteComment(Comment comment)
        {
            _context.Remove(comment);
            return await Save();
        }

        public async Task<Comment> GetCommentById(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> Save()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
