using Microsoft.EntityFrameworkCore;

namespace CoreApi.Infrastructure.Database
{
    public sealed class CoreApiContext : DbContext
    {
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
        }
    }
}