using LearnAndRepeatWeb.Contracts.Responses.Card;

namespace LearnAndRepeatWeb.Contracts.Events.Card
{
    public class CardCreatedEvent
    {
        public CardResponse CardResponse { get; set; }
    }
}
