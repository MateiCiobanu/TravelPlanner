using Microsoft.EntityFrameworkCore;
using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Domain.Models;
using TravelPlanner.Infrastructure.Persistence;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(User user)
        {
            _context.Add(user);
            return await Save();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

        }

        public async Task<User> GetUserByUsername(string userName)
        {
            return await _context.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        }
        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
