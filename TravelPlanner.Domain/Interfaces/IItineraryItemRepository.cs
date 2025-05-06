using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Domain.Interfaces
{
    public interface IItineraryItemRepository
    {
        Task<IEnumerable<ItineraryItem>> GetByTripIdAsync(int tripId);
        Task<ItineraryItem> GetByIdAsync(int id);
        Task<IEnumerable<ItineraryItem>> AddRangeAsync(IEnumerable<ItineraryItem> items);
        Task<ItineraryItem> CreateAsync(ItineraryItem item);
        Task<ItineraryItem> UpdateAsync(ItineraryItem item);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteByTripIdAsync(int tripId);
    }
}
