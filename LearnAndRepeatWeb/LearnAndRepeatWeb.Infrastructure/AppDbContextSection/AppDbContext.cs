using LearnAndRepeatWeb.Infrastructure.Entities.Card;
using LearnAndRepeatWeb.Infrastructure.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace LearnAndRepeatWeb.Infrastructure.AppDbContextSection
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<CardModel>().HasQueryFilter(m => !m.IsDeleted);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserModel).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserModel> User { get; set; }
        public DbSet<UserTokenModel> UserToken { get; set; }
        public DbSet<CardModel> Card { get; set; }
    }
}
