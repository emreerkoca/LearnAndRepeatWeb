using LearnAndRepeatWeb.Infrastructure.AppDbContextSection;
using LearnAndRepeatWeb.Infrastructure.Entities.Card;

namespace LearnAndRepeatWeb.Infrastructure.Repositories.Card
{
    public class CardRepository : Repository<CardModel>, ICardRepository
    {
        public CardRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
