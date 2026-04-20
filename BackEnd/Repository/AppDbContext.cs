using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Courier> Couriers { get; set; } 

        public DbSet<Order> Orders { get; set; }
    }
}
