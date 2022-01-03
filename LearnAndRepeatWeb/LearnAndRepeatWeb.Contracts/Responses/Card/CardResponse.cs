using System;

namespace LearnAndRepeatWeb.Contracts.Responses.Card
{
    public class CardResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string Tag { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
