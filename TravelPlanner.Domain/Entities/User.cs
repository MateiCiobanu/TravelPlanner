using TravelPlanner.Domain.Entities;

namespace TravelPlanner.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<TravelerType> TravelerTypes { get; set; }
        public ICollection<Trip> Trips { get; set; }
    }
}
