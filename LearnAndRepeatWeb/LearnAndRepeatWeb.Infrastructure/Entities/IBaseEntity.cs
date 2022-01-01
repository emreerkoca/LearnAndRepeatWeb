using System;

namespace LearnAndRepeatWeb.Infrastructure.Entities
{
    public interface IBaseEntity
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
