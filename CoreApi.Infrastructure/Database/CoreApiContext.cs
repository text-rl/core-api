using CoreApi.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreApi.Infrastructure.Database
{
    public sealed class CoreApiContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public CoreApiContext(DbContextOptions<CoreApiContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("user")
                    .Property(p => p.Id)
                    .ValueGeneratedNever();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}