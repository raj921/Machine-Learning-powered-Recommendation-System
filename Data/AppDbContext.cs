using Microsoft.EntityFrameworkCore;
using RecommendationSystem.Models;

namespace RecommendationSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
    }
}