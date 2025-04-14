using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Domain.Models;
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

        public async Task<IEnumerable<Trip>> GetAllAsync()
        {
            return await _context.Trips
                .Include(t => t.ItineraryItems)
                .ToListAsync();
        }

        public async Task<IEnumerable<Trip>> GetByUserIdAsync(int userId)
        {
            return await _context.Trips
                .Where(t => t.UserId == userId)
                .Include(t => t.ItineraryItems)
                .ToListAsync();
        }

        public async Task<Trip> GetByIdAsync(int id)
        {
            return await _context.Trips
                .Include(t => t.ItineraryItems)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Trip> CreateAsync(Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
            return trip;
        }

        public async Task<Trip> UpdateAsync(Trip trip)
        {
            _context.Entry(trip).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return trip;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null)
                return false;

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}