using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TravelPlanner.Domain.Entities;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Infrastructure.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<User> Users { get; set; }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<ItineraryItem> ItineraryItems { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }

    }
}
