using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelPlanner.Application.DTOs
{
    public class TravelerTypeCreateDTO
    {
        public string TravelerTypeName { get; set; }
        public int PreferenceWeight { get; set; } = 5;
    }
}
