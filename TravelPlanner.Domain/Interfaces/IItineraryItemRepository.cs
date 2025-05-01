using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Domain.Interfaces
{
    public interface IItineraryItemRepository
    {
        Task<bool> CreateItineraryItem(ItineraryItem itineraryItem);
        Task<bool> Save();
    }
}
