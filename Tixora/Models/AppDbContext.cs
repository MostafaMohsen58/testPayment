using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Tixora.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<booking> Bookings { get; set; }
        public DbSet<Event> Events { get; set; }
    }
    
}
