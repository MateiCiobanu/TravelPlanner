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
    public class TripRepository : ITripRepository
    {
        private readonly DataContext _context;
        public TripRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateTrip(Trip trip)
        {
            _context.Add(trip);
            return await Save();
        }
        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
    {
    }
}
