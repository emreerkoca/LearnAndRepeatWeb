using System;

namespace LearnAndRepeatWeb.Core.Entities.Card
{
    public class Card : BaseEntity
    {
        public long UserId { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string Tag { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
