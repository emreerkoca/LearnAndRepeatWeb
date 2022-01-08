using LearnAndRepeatWeb.Infrastructure.AppDbContextSection;
using LearnAndRepeatWeb.Infrastructure.Entities.User;

namespace LearnAndRepeatWeb.Infrastructure.Repositories.User
{
    public class UserRepository : Repository<UserModel>, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
