using LearnAndRepeatWeb.Infrastructure.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace LearnAndRepeatWeb.Infrastructure.AppDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> User { get; set; }
    }
}
