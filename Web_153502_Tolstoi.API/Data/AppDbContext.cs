using Microsoft.EntityFrameworkCore;
using Web_153502_Tolstoi.Domain.Entities;

namespace Web_153502_Tolstoi.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Game> Games { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
