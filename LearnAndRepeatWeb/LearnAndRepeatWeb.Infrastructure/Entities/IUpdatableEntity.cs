using System;

namespace LearnAndRepeatWeb.Infrastructure.Entities
{
    public interface IUpdatableEntity
    {
        public DateTime UpdateDate { get; set; }
    }
}
