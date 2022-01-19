using System.Collections.Generic;

namespace LearnAndRepeatWeb.Contracts.Responses.Card
{
    public class CardListResponse
    {
        public List<CardResponse> CardResponseList { get; set; }
        public int TotalItemCount { get; set; }
    }
}
