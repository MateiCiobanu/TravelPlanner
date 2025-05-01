using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanner.Domain.Entities;
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

        public async Task<bool> CreateItineraryItem(ItineraryItem itineraryItem)
        {
            _context.Add(itineraryItem);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
