using Microsoft.EntityFrameworkCore;
using zad2.Models;

namespace zad2.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Password> Passwords { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("USER");
            modelBuilder.Entity<Password>().ToTable("PASSWORD");
            
        }
    }
}
