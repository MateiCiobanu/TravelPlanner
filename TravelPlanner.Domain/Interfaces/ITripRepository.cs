
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Domain.Interfaces
{
    public interface ITripRepository
    {
        Task<IEnumerable<Trip>> GetAllAsync();
        Task<IEnumerable<Trip>> GetByUserIdAsync(int userId);
        Task<Trip> GetByIdAsync(int id);
        Task<Trip> CreateAsync(Trip trip);
        Task<Trip> UpdateAsync(Trip trip);
        Task<bool> DeleteAsync(int id);
    }
}
