using Microsoft.EntityFrameworkCore;
using PrsBackEnd.Models;

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

        // constructor to support dependency injection (via a service)
        public DbSet<PrsBackEnd.Models.Request> Requests { get; set; }

        // constructor to support dependency injection (via a service)
        public DbSet<PrsBackEnd.Models.RequestLine> RequestLines { get; set; }
    }
}
