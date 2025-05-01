using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Domain.Interfaces
{
    public interface ITripRepository
    {
        Task<bool> CreateTrip(Trip trip);
        Task<bool> Save();
    }
}
