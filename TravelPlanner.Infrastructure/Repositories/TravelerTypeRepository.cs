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
    public class TravelerTypeRepository : ITravelerTypeRepository
    {
        private DataContext _context;
        public TravelerTypeRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateTravelerType(TravelerType travelerType)
        {
            _context.Add(travelerType);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
