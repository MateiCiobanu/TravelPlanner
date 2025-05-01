using TravelPlanner.Domain.Entities;

namespace TravelPlanner.Domain.Interfaces
{
    public interface ITravelerTypeRepository
    {
        Task<bool> CreateTravelerType(TravelerType travelerType);
        Task<bool> Save();
    }
}
