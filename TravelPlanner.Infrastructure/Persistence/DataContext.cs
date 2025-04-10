using Microsoft.EntityFrameworkCore;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Infrastructure.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<User> Users { get; set; }

    }
}
