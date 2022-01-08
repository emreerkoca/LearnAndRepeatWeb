using LearnAndRepeatWeb.Infrastructure.AppDbContextSection;
using LearnAndRepeatWeb.Infrastructure.Entities.User;

namespace LearnAndRepeatWeb.Infrastructure.Repositories.User
{
    public class UserTokenRepository : Repository<UserTokenModel>, IUserTokenRepository 
    {
        public UserTokenRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
