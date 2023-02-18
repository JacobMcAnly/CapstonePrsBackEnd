using Microsoft.EntityFrameworkCore;

namespace PrsBackEnd.Models
{
    public class PrsDbContext : DbContext  // Not a POCO
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        // constructor to support dependency injection (via a service)
        public PrsDbContext(DbContextOptions<PrsDbContext> options) : base(options)
        {
        }
    }
}
