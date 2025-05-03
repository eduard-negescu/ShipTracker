
using Microsoft.EntityFrameworkCore;
using ShipTracker.Server.Models.Entities;

namespace ShipTracker.Server.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }

    }
}
