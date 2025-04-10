using TravelPlanner.Domain.Models;

namespace TravelPlanner.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserById(int id);
        Task<bool> CreateUser(User user);
        Task<bool> Save();
    }
}
