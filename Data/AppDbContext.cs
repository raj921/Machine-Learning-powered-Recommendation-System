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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add some sample data
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "User 1", Email = "user1@example.com" },
                new User { Id = 2, Name = "User 2", Email = "user2@example.com" }
            );

            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, Name = "Item 1", Description = "Description 1" },
                new Item { Id = 2, Name = "Item 2", Description = "Description 2" }
            );

            modelBuilder.Entity<Interaction>().HasData(
                new Interaction { Id = 1, UserId = 1, ItemId = 1, Rating = 4.5f, Timestamp = DateTime.UtcNow },
                new Interaction { Id = 2, UserId = 1, ItemId = 2, Rating = 3.5f, Timestamp = DateTime.UtcNow },
                new Interaction { Id = 3, UserId = 2, ItemId = 1, Rating = 5.0f, Timestamp = DateTime.UtcNow }
            );
        }
    }
}
