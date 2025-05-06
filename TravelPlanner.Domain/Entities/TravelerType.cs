using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Domain.Entities
{
    public class TravelerType
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string TravelerTypeName { get; set; }

        public int PreferenceWeight { get; set; } = 5;
    }
}
