using System;

namespace LearnAndRepeatWeb.Infrastructure.Entities
{
    public interface ISoftDeletableEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime DeleteDate { get; set; }
    }
}
