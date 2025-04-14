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
    public class ItineraryItemRepository : IItineraryItemRepository
    {
        private readonly DataContext _context;

        public ItineraryItemRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItineraryItem>> GetByTripIdAsync(int tripId)
        {
            return await _context.ItineraryItems
                .Where(i => i.TripId == tripId)
                .OrderBy(i => i.DayNumber)
                .ThenBy(i => i.StartTime)
                .ToListAsync();
        }

        public async Task<ItineraryItem> GetByIdAsync(int id)
        {
            return await _context.ItineraryItems.FindAsync(id);
        }

        public async Task<IEnumerable<ItineraryItem>> AddRangeAsync(IEnumerable<ItineraryItem> items)
        {
            _context.ItineraryItems.AddRange(items);
            await _context.SaveChangesAsync();
            return items;
        }

        public async Task<ItineraryItem> CreateAsync(ItineraryItem item)
        {
            _context.ItineraryItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<ItineraryItem> UpdateAsync(ItineraryItem item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.ItineraryItems.FindAsync(id);
            if (item == null)
                return false;

            _context.ItineraryItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByTripIdAsync(int tripId)
        {
            var items = await _context.ItineraryItems
                .Where(i => i.TripId == tripId)
                .ToListAsync();

            if (!items.Any())
                return false;

            _context.ItineraryItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}